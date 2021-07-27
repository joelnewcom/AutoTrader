using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoTrader.Data
{
    public class AssetPair
    {
        public string Id { get; }

        public string Name { get; }

        public int Accuracy { get; }


        public AssetPair(String id, String name, int accuracy)
        {
            this.Id = id;
            this.Name = name;
            this.Accuracy = accuracy;
        }
    }
}