using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;

namespace USYD_demo
{
    [Serializable]
    public  class Constants
    {
        #region Luis Entities
        public static string LuisEntity_PoNumber = "PONumber";
        public static string LuisEntity_InvoiceNumber = "InvoiceNumber";
        public static string LuisEntity_AccountNumber = "AccountNumber";
        #endregion

        #region Common constants
        public static string POConfirmPromptText = "Is there anything else that we can assist you with today?";
        public static string FeedbackPromptText = "Do you want to provide additional feedback to improve the capabilities of the FinBot?";

        public static string SubmitFeedbackText = "Thanks for your feedback!";
        #endregion
    }
}