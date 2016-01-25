using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wander.Server.ClassLibrary.Services.Interfaces;
using Wander.Server.ClassLibrary.Utilities;

namespace Wander.Server.ClassLibrary.Services
{
    public class MapService : IMapService
    {
        private int _mapWidth, _mapHeight, _tileWidth, _tileHeight;
        private byte[] _map;

        public MapService()
        {
            LoadMap();
        }
        public byte[] GetMap()
        {
            return _map;
        }

        public bool IsCollision(int x, int y)
        {
            if (x < 0 || x >= this._mapWidth || y < 0 || y >= this._mapHeight)
            {
                return true;
            }
            return _map[y * this._mapHeight + x] == 1;
        }

        public int GetWidth()
        {
            return _mapWidth;
        }

        public int GetHeight()
        {
            return _mapHeight;
        }

        public int GetTileWidth()
        {
            return _tileWidth;
        }

        public int GetTileHeight()
        {
            return _tileHeight;
        }

        private string DownloadMap()
        {
            string contents;
            using (var wc = new System.Net.WebClient())
                contents = wc.DownloadString(Variables.DistantServerUrl + "/Content/Game/Maps/map2.json");

            return contents;
        }

        private void LoadMap()
        {
            return;

            ServicePointManager
    .ServerCertificateValidationCallback +=
    (sender, cert, chain, sslPolicyErrors) => true;

            string mapContent = "";

            if (Directory.Exists(Path.GetTempPath() + "Wander"))
            {
                Debug.Print("cache found");
                if (File.Exists(Path.GetTempPath() +"Wander/"+ "map2.json"))
                {
                    Debug.Print("Map found in cache");
                    using (WebClient client = new WebClient())
                    {
                        string distantHash = client.DownloadString(Variables.DistantServerUrl + "/mapHash.aspx");
                        using (var md5 = MD5.Create())
                        {

                            using (var stream = File.OpenRead(Path.GetTempPath() + "map2.json"))
                            {
                                var t = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");

                                string currentHash = "";
                                t.ForEach(x => currentHash += x.ToString());
                                mapContent = currentHash != distantHash ? DownloadMap() : File.ReadAllText(Path.GetTempPath() + "map2.json");
                            }
                        }
                    }
                }
                else
                {
                    mapContent = DownloadMap();
                    TextWriter tw = new StreamWriter(Path.GetTempPath() + "Wander/map2.json", true);
                    tw.Write(mapContent);
                    tw.Close();
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetTempPath() + "Wander");
                mapContent = DownloadMap();
                TextWriter tw = new StreamWriter(Path.GetTempPath() + "Wander", true);
                tw.Write(mapContent);
                tw.Close();
            }

            if (string.IsNullOrEmpty(mapContent))
            {
                throw new ArgumentException("Couldnt load map !");
            }

            JObject map = JsonConvert.DeserializeObject<JObject>(mapContent);
            this._mapWidth = Convert.ToInt32(map["width"].ToString());
            this._mapHeight = Convert.ToInt32(map["width"].ToString());
            _tileHeight = Convert.ToInt32(map["tileheight"].ToString());
            _tileWidth = _tileHeight;
            _map = new byte[_mapWidth * _mapHeight];
            var layers = map["layers"];
            
            foreach (var i in layers)
            {
                string layerName = i["name"].ToString();
                Debug.Print(layerName);
                var layerData = i["data"];

                if (layerName == "backgroundLayer")
                {

                    int counter = 0;
                    layerData.ForEach(x =>
                    {
                        _map[counter] = 0;
                        counter++;

                    });
                }

                else if (layerName == "collisionLayer")
                {
                    int counter = 0;
                    layerData.ForEach(x =>
                    {
                        if ((long)(x) != 0)
                        {
                            _map[counter] = 1;
                        }

                        counter++;

                    });

                }
            }

        }
    }
}
