using System;

namespace AutoTrader.Data
{
    public class AssetPair : IEquatable<AssetPair>
    {
        public String Id { get; }

        public String Name { get; private set; }

        public int Accuracy { get; private set; }

        public String BaseAssetId { get; private set; }

        public String QuotingAssetId { get; private set; }

        public AssetPair(String id, String name, int accuracy, String baseAssetId, String quotingAssetId)
        {
            this.Id = id;
            this.Name = name;
            this.Accuracy = accuracy;
            this.BaseAssetId = baseAssetId;
            this.QuotingAssetId = quotingAssetId;
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