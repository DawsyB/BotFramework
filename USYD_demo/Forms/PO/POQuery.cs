using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace USYD_demo
{
    [Serializable]
    public class POQuery
    {
        public  string AccountNumber { get; set; }
        public  string PONumber { get; set; }
        public  string UniKey { get; set; }

        
    }
}