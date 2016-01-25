using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wander.Server.ClassLibrary.Services.Interfaces;

namespace Wander.Server.ClassLibrary.Services
{
    public class MapService : IMapService
    {
        private int _mapWidth, _mapHeight;
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
            throw new NotImplementedException();
        }

        public int GetTileHeight()
        {
            throw new NotImplementedException();
        }

        private void LoadMap()
        {
            string contents;
            using (var wc = new System.Net.WebClient())
                contents = wc.DownloadString("http://wander.nightlydev.fr/Content/Game/Maps/map2.json");
            JObject map = JsonConvert.DeserializeObject<JObject>(contents);
            this._mapWidth = Convert.ToInt32(map["width"].ToString());
            this._mapHeight = Convert.ToInt32(map["width"].ToString());
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
