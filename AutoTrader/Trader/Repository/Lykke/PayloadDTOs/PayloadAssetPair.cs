namespace AutoTrader.Repository
{
    public class PayloadAssetPair
    {
        public string assetPairId { get; set; }
        public string baseAssetId { get; set; }
        public string quoteAssetId { get; set; }
        public string name { get; set; }
        public int priceAccuracy { get; set; }
        public int baseAssetAccuracy { get; set; }
        public int quoteAssetAccuracy { get; set; }
        public float minVolume { get; set; }
        public float minOppositeVolume { get; set; }


    }
}