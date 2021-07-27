using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoTrader.Data
{
    public interface IDataAccess
    {
        AssetPair AddAssetPairHistoryEntry(AssetPair assetPair, AssetPairHistoryEntry assetPairHistoryEntry);

        List<AssetPairHistoryEntry> GetAssetPairHistory(AssetPair assetPair);

        List<float> GetBidHistory(AssetPair assetPair);

        List<float> GetAskHistory(AssetPair assetPair);

        DateTime GetYoungestDate(AssetPair assetPair);
    }
}
