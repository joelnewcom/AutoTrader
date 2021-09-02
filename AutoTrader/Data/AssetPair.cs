using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoTrader.Data
{
    public class AssetPair : IEquatable<AssetPair>
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

        public override bool Equals(object obj)
        {
            return Equals(obj as AssetPair);
        }

        public bool Equals(AssetPair other)
        {
            return other != null &&
                Id == other.Id &&
                Name == other.Name &&
                Accuracy == other.Accuracy;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Accuracy);
        }
    }
}