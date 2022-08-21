namespace AutoTrader.Models
{
    public class TradeFee
    {
        public float size { get; set; }
        public string assetId { get; set; }
        public TradeFee(float size, string assetId)
        {
            this.size = size;
            this.assetId = assetId;
        }
    }
}