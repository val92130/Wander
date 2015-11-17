using Cake.Common;
using Cake.Common.Solution;
using Cake.Common.IO;
using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.NuGet;
using Cake.Core;
using Cake.Common.Diagnostics;
using Code.Cake;
using Cake.Common.Tools.NuGet.Pack;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Common.Tools.NuGet.Restore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;
using Cake.Common.Build;
using Cake.Common.Tools.NuGet.Push;
using Cake.Common.Tools.MSTest;


namespace CodeCake
{
    using Cake.Common.Tools.Cake;
    using Cake.Core.IO;
    using System.IO;

    /// <summary>
    /// Sample build "script".
    /// Build scripts can be decorated with AddPath attributes that inject existing paths into the PATH environment variable. 
    /// </summary>
    [AddPath("CodeCakeBuilder/Tools")]
    public class Build : CodeCakeHost
    {
        public Build()
        {
            // The configuration ("Debug", etc.) defaults to "Release".
            var configuration = Cake.Argument("configuration", "Release");

            // Git .ignore file should ignore this folder.
            // Here, we name it "Releases" (default , it could be "Artefacts", "Publish" or anything else, 
            // but "Releases" is by default ignored in https://github.com/github/gitignore/blob/master/VisualStudio.gitignore.
            var releasesDir = Cake.Directory("CodeCakeBuilder/Releases");

            Task("Clean")
                .Does(() =>
                {
                    // Avoids cleaning CodeCakeBuilder itself!
                    Cake.CleanDirectories("**/bin/" + configuration, d => !d.Path.Segments.Contains("CodeCakeBuilder"));
                    Cake.CleanDirectories("**/obj/" + configuration, d => !d.Path.Segments.Contains("CodeCakeBuilder"));
                    Cake.CleanDirectories(releasesDir);
                });

            Task("Restore-NuGet-Packages")
                .Does(() =>
                {
                    Cake.Information("Restoring nuget packages for existing .sln files at the root level.", configuration);
                    foreach (var sln in Cake.GetFiles("*.sln"))
                    {
                        Cake.NuGetRestore(sln);
                    }
                });

            Task("Build")
                .IsDependentOn("Clean")
                .IsDependentOn("Restore-NuGet-Packages")
                .Does(() =>
                {
                    Cake.Information("Building all existing .sln files at the root level with '{0}' configuration (excluding this builder application).", configuration);
                    foreach (var sln in Cake.GetFiles("*.sln"))
                    {
                        using (var tempSln = Cake.CreateTemporarySolutionFile(sln))
                        {
                            // Excludes "CodeCakeBuilder" itself from compilation!
                            tempSln.ExcludeProjectsFromBuild("CodeCakeBuilder");
                            Cake.MSBuild(tempSln.FullPath, new MSBuildSettings()
                                    .SetConfiguration(configuration)
                                    .SetVerbosity(Verbosity.Minimal)
                                    .SetMaxCpuCount(1));
                        }
                    }
                });
            Task("CopyFiles")
            .IsDependentOn("Unit-Tests")
            .Does(() =>
            {
                if (Cake.AppVeyor().IsRunningOnAppVeyor)
                {
                    var path = "./Wander.Server/";
                    var files = Cake.GetFiles(path + "*");
                    string tmp = "./tmp";

                    if (!Directory.Exists(tmp))
                    {
                        Directory.CreateDirectory(tmp);
                    }
                    else
                    {
                        Directory.Delete(tmp, true);
                        Directory.CreateDirectory(tmp);
                    }
                    DirectoryPath path2 = new DirectoryPath(path);
                    DirectoryPath d = new DirectoryPath(tmp + "/");
                    this.Cake.CopyDirectory(path2, d);

                    Directory.Delete(tmp + "/Properties", true);
                    Directory.Delete(tmp + "/obj", true);
                    Directory.Delete(tmp + "/Model", true);
                    Directory.Delete(tmp + "/Hubs", true);
                    this.Cake.DeleteFiles(tmp + "/*.cs");
                    this.Cake.DeleteFiles(tmp + "/*.csproj");

                }
                else
                {
                    Cake.Information("Not running on appveyor, skipping...");
                }
            });
            Task("Package")
            .IsDependentOn("CopyFiles")
            .Does(() =>
            {
                if (Cake.AppVeyor().IsRunningOnAppVeyor)
                {
                    string pswd = Environment.GetEnvironmentVariable("FTP_PASSWORD");
                    if (pswd != null)
                    {
                        string output = "./output";
                        if (!Directory.Exists(output))
                        {
                            Directory.CreateDirectory(output);
                        }
                        else
                        {
                            Directory.Delete(output, true);
                            Directory.CreateDirectory(output);
                        }
                        string tmp = "./tmp";
                        Cake.Information("Zipping...");

                        Cake.Zip(tmp + "/", output + "/build.zip");


                        Cake.Information("Deploying");
                        FileStream stream = null;
                        Stream reqStream = null;
                        try
                        {
                            FtpWebRequest request =
                                (FtpWebRequest)FtpWebRequest.Create("ftp://labo.nightlydev.fr/" + "/www/" + "build.zip");
                            request.Method = WebRequestMethods.Ftp.UploadFile;
                            request.Credentials = new NetworkCredential("administrateur", "pass");
                            request.UsePassive = true;
                            request.UseBinary = true;
                            request.KeepAlive = false;

                            Cake.Information("Uploading to server...");
                            stream = File.OpenRead(output + "/build.zip");
                            byte[] buffer = new byte[stream.Length];

                            reqStream = request.GetRequestStream();
                            reqStream.Write(buffer, 0, buffer.Length);
                            reqStream.Close();
                        }
                        catch (Exception e)
                        {
                            Cake.Information(e.Message);
                        }
                        finally
                        {
                            if (reqStream != null) reqStream.Close();
                            if (stream != null) stream.Close();

                            Directory.Delete(tmp, true);
                            Directory.Delete(output, true);

                        }
                    }
                    
                }
                else
                {
                    Cake.Information("Not running on appveyor, skipping...");
                }

            });


            Task("DBSetup")
                .IsDependentOn("Build")
                .Does(
                    () =>
                    {
                        string db = Cake.InteractiveEnvironmentVariable("DB_CONNECTION_STRING");
                        if (Cake.AppVeyor().IsRunningOnAppVeyor)
                        {
                            db = @"Server=(local)\SQL2014;Database=master;User ID=sa;Password=Password12!";
                        }

                        if (String.IsNullOrEmpty(db))
                            db =
                                @"Data Source=(localdb)\ProjectsV12;Initial Catalog=WanderDB;Integrated Security=True;";
                        Cake.Information("Using database: {0}", db);

                        SqlConnection conn = new SqlConnection(db);

                        string filePath = "Docs/BDD/create.sql";
                        if (System.IO.File.Exists(filePath))
                        {
                            string create = System.IO.File.ReadAllText(filePath);
                            string[] commands = create.Split(new string[] { "GO" }, StringSplitOptions.None);

                            IEnumerable<string> commandStrings = Regex.Split(create, @"^\s*GO\s*$",
                           RegexOptions.Multiline | RegexOptions.IgnoreCase);

                            conn.Open();
                            foreach (string commandString in commandStrings)
                            {
                                if (commandString.Trim() != "")
                                {
                                    using (var command = new SqlCommand(commandString, conn))
                                    {
                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                            conn.Close();
                        }

                    });

            Task("Unit-Tests")
                .IsDependentOn("DBSetup")
                .Does(() =>
                {
                    Cake.MSTest("**/bin/" + configuration + "/*.Tests.dll");
                });


            Task("Default").IsDependentOn("Package");

        }
    }
}
