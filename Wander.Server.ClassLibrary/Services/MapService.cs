using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wander.Server.ClassLibrary.Services.Interfaces;
using Wander.Server.ClassLibrary.Utilities;

namespace Wander.Server.ClassLibrary.Services
{
    public class MapService : IMapService
    {
        private byte[] _map;
        private int _mapWidth, _mapHeight, _tileWidth, _tileHeight;

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
            if (x < 0 || x >= _mapWidth || y < 0 || y >= _mapHeight)
            {
                return true;
            }
            return _map[y*_mapHeight + x] == 1;
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
            using (var wc = new WebClient())
                contents = wc.DownloadString(Variables.DistantServerUrl + "/Content/Game/Maps/map2.json");

            return contents;
        }

        private void LoadMap()
        {
            return;

            var mapContent = "";

            if (Directory.Exists(Path.GetTempPath() + "Wander"))
            {
                Debug.Print("cache found");
                if (File.Exists(Path.GetTempPath() + "Wander/" + "map2.json"))
                {
                    Debug.Print("Map found in cache");
                    using (var client = new WebClient())
                    {
                        var distantHash = client.DownloadString(Variables.DistantServerUrl + "/mapHash.aspx");
                        using (var md5 = MD5.Create())
                        {
                            using (var stream = File.OpenRead(Path.GetTempPath() + "map2.json"))
                            {
                                var t = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");

                                var currentHash = "";
                                t.ForEach(x => currentHash += x.ToString());
                                mapContent = currentHash != distantHash
                                    ? DownloadMap()
                                    : File.ReadAllText(Path.GetTempPath() + "map2.json");
                            }
                        }
                    }
                }
                mapContent = DownloadMap();
                
                var writer = new StreamWriter(Path.GetTempPath() + "Wander/map2.json", true);
                writer.Write(mapContent);
                writer.Close();
            }
            Directory.CreateDirectory(Path.GetTempPath() + "Wander");
            mapContent = DownloadMap();
            TextWriter tw = new StreamWriter(Path.GetTempPath() + "Wander", true);
            tw.Write(mapContent);
            tw.Close();

            if (string.IsNullOrEmpty(mapContent))
            {
                throw new ArgumentException("Couldnt load map !");
            }

            var map = JsonConvert.DeserializeObject<JObject>(mapContent);
            _mapWidth = Convert.ToInt32(map["width"].ToString());
            _mapHeight = Convert.ToInt32(map["width"].ToString());
            _tileHeight = Convert.ToInt32(map["tileheight"].ToString());
            _tileWidth = _tileHeight;
            _map = new byte[_mapWidth*_mapHeight];
            var layers = map["layers"];

            foreach (var i in layers)
            {
                var layerName = i["name"].ToString();
                Debug.Print(layerName);
                var layerData = i["data"];

                if (layerName == "backgroundLayer")
                {
                    var counter = 0;
                    layerData.ForEach(x =>
                    {
                        _map[counter] = 0;
                        counter++;
                    });
                }

                if (layerName == "collisionLayer")
                {
                    var counter = 0;
                    layerData.ForEach(x =>
                    {
                        if ((long) (x) != 0)
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