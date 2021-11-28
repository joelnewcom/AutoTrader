using System;

namespace AutoTrader.Data
{
    public class AssetPair : IEquatable<AssetPair>
    {
        public string Id { get; }

        public string Name { get; }

        public int Accuracy { get; }

        public string baseAssetId { get; }

        public string quotingAssetId { get; }

        public AssetPair(String id, String name, int accuracy, string baseAssetId, string quotingAssetId)
        {
            this.Id = id;
            this.Name = name;
            this.Accuracy = accuracy;
            this.baseAssetId = baseAssetId;
            this.quotingAssetId = quotingAssetId;
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