using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoTrader.Repository
{
    public class PayloadGetHistoryRate
    {
        public string Period { get; set; }
        public DateTime DateTime { get; set; }
    }
}
