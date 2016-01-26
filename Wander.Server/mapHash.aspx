<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mapHash.aspx.cs" Inherits="Wander.Server.mapHash" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Security.Cryptography" %>
<%@ Import Namespace="Wander.Server.ClassLibrary" %>


 <% 
using (var md5 = MD5.Create())
{
         
    using (var stream = File.OpenRead(Server.MapPath("/Content/Game/Maps/") + "map2.json"))
    {
        var t = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");

        string hash = "";
        t.ForEach(x => hash += x.ToString());
        Response.Write(hash);
    }
}   
%>
