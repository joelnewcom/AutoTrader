using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoTrader.Data
{
    public interface IDataAccess
    {
        AssetPair AddAssetPairHistoryEntry(AssetPair assetPair, IAssetPairHistoryEntry assetPairHistoryEntry);

        List<IAssetPairHistoryEntry> GetAssetPairHistory(AssetPair assetPair);

        List<float> GetBidHistory(AssetPair assetPair);

        List<float> GetAskHistory(AssetPair assetPair);

        DateTime GetYoungestDate(AssetPair assetPair);

        List<AssetPair> GetAssetPairs();
    }
}
