using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace USYD_demo.Dialogs
{
    [LuisModel("5cd7129d-973f-423d-a071-e726a8f230cb", "8f6d88e12e98423e8e74401ace73c4f4")]
    [Serializable]
    public class InvoiceDialog : LuisDialog<object>
    {
        private const string LuisEntity_PoNumber = "PONumber";
        private const string LuisEntity_InvoiceNumber = "InvoiceNumber";
        private const string LuisEntity_number = "builtin.number";
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> querymessage, LuisResult result)
        {
            var message = await querymessage;
            string Displaymessage = ($"Message received so far -- {message.Text} - {result.Query}");
            
            await context.PostAsync(Displaymessage);
            await context.PostAsync("What sort of information are you after in regards to invoices?");
            
        }

        
        [LuisIntent("ShowInvoiceDetails")]
        public async Task Invoice(IDialogContext context, IAwaitable<IMessageActivity> querymessage, LuisResult result)
        {
            var message = await querymessage;
            EntityRecommendation PoNumber;
            EntityRecommendation number;
            result.TryFindEntity(LuisEntity_PoNumber, out PoNumber);
            result.TryFindEntity(LuisEntity_number, out number);
            string Displaymessage = ($"Reached Invoice None Stream - {message.Text} - {result.Query}");
            await context.PostAsync(Displaymessage);
            
        }
    }
}