using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Repository;

namespace AutoTrader.Advisor
{
    public class BuyIfEnoughCHFAsset : IAsyncAdvisor<float>
    {

        IRepository repo;
        public BuyIfEnoughCHFAsset(IRepository lykkeRepository)
        {
            this.repo = lykkeRepository;
        }

        public async Task<Advice> advice(float volume)
        {
            List<IWalletEntry> walletEntries = await repo.GetWallets();

            foreach (IWalletEntry item in walletEntries)
            {
                if ("CHF".Equals(item.AssetId) && item.Balance > volume)
                {
                    return Advice.Buy;
                }
            }

            return Advice.HoldOn;
        }
    }
}