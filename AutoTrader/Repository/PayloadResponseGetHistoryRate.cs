﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoTrader.Repository
{
    public class PayloadResponseGetHistoryRate
    {
        public string Id { get; set; }
        public float Bid { get; set; }
        public float Ask { get; set; }
        public float TradingVolume { get; set; }
        public float TradingOppositeVolume { get; set; }
    }
}
