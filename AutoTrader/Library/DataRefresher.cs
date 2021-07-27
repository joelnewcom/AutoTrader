using AutoTrader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTrader.Data;

namespace AutoTrader.Library
{
    public class DataRefresher
    {
        private IRepository repository;
        private IDataAccess data;

        public DataRefresher(IRepository repository, IDataAccess data)
        {
            this.repository = repository;
            this.data = data;
        }
       
        public void refreshAssetPairHistory(AssetPair assetPair){
            DateTime youngestDate = data.GetYoungestDate(assetPair);
            if (DateTime.Today > youngestDate){
                repository
            }
            
        }
    }
}
