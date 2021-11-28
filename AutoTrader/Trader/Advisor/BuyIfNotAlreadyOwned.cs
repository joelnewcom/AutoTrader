using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Repository;

namespace AutoTrader.Advisor
{
    public class BuyIfNotAlreadyOwned : IAsyncAdvisor<String>
    {

        IRepository repo;
        public BuyIfNotAlreadyOwned(IRepository lykkeRepository)
        {
            this.repo = lykkeRepository;
        }

        public async Task<Advice> advice(string assetId)
        {
            List<IWalletEntry> walletEntries = await repo.GetWallets();

            foreach (IWalletEntry item in walletEntries)
            {
                if (assetId.Equals(item.AssetId))
                {
                    return Advice.HoldOn;
                }
            }

            return Advice.Buy;
        }
    }
}