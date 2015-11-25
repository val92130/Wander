using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.Model;

namespace Wander.Server.Lib.Model
{
    
    public class MoneyBag
    {
        public static int LastId = 0;
        public int Ammount { get; set; }
        public Vector2 Position { get; set; }
        public int Id { get; set; }

        public MoneyBag(Vector2 position, int ammount)
        {
            LastId++;
            this.Ammount = ammount;
            this.Position = position;
            this.Id = LastId;
        }
    }
}
