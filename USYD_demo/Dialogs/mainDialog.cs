using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace USYD_demo.Dialogs
{
    [LuisModel("5cd7129d-973f-423d-a071-e726a8f230cb", "8f6d88e12e98423e8e74401ace73c4f4")]
    [Serializable]
    public class mainDialog:LuisDialog<object>
    {
        public const string GreeintingDone = "isGreetingDone";
        public const string RecentQuery = "";
       

        [LuisIntent("")]
        [LuisIntent("None")]    
        public async Task None(IDialogContext context, IAwaitable<IMessageActivity> querymessage, LuisResult result)
        {
            //string message = $"Sorry, We did not understand '{result.Query}'. OR Type 'Accounts query' or 'Account balance' or 'Purchase orders' or 'Invoices' to get started.";
            string message = ($"No results found for '{result.Query}'. Looking in knowledge base!!!");
            await context.PostAsync(message);
            var messageToForward = await querymessage;
            await context.Forward(new QnADialog(), this.ResumeAfterNone, messageToForward, CancellationToken.None);
            
            context.Wait(this.MessageReceived);
        }


        


        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context,IAwaitable<IMessageActivity>activity, LuisResult result)
        {
            var message = await activity;
            bool completedGreeting = false;
            context.UserData.TryGetValue(GreeintingDone, out completedGreeting);
            if (!completedGreeting)
            {
                await context.PostAsync("Welcome to USYD Demo!!");
                await context.PostAsync("Lets get Started! How do you want us to help you today?");
                context.UserData.SetValue<bool>("isGreetingDone", true);                
            }
            else
            {
                await context.PostAsync($"Hey {message.From.Name}({message.From.Id}), is there anything else that we can help you with?");
                    }
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("AccountsPayable")]
        public async Task AccountsPayable(IDialogContext context, LuisResult result)
        {
            
            await context.PostAsync("You have reached Accounts payable help desk!");            
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("PurchaseOrder")]
        public async Task PurchaseOrder(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("You have reached Purchase Order help desk!");
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Invoice")]
        public async Task Invoice(IDialogContext context, IAwaitable<IMessageActivity> querymessage,LuisResult result)
        {
            await context.PostAsync("Welcome to Invoice Stream USYD help desk!");
            await context.Forward(new InvoiceDialog(), this.ResumeAfterInvoiceDialog, await querymessage, CancellationToken.None);
            //context.Wait(this.MessageReceived);
        }


        #region ResumeAfter methods
        private async Task ResumeAfterNone(IDialogContext context, IAwaitable<object> result)
        {
            
            bool MaxOutAttempts = false;
            context.ConversationData.TryGetValue<bool>("MaxedOutAttempts", out MaxOutAttempts);
            if (MaxOutAttempts)
            {
                PromptDialog.Confirm(context, this.ResumeAfterMaxedOutAttemptsPrompt, "We noticed you have problems getting around! Do you want to show us menu driven process?","Please retry?");
                
            }
            else
            {
                await context.PostAsync("Hope, this information was useful!");
                context.Done<object>(null);
            }
        }

        private async Task ResumeAfterMaxedOutAttemptsPrompt(IDialogContext context, IAwaitable<bool> result)
        {
            var toContinue = await result;
            if (toContinue)
            {
                await context.PostAsync("Show menu driven options here");
                //todo: Show manual options 
            }
            else
            {
                await context.PostAsync("Lets start again!");
                context.Done<object>(null);
            }
        }

        private async Task ResumeAfterInvoiceDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Reached Invoice Resume after dialog method");
            context.Done<object>(null);
        }
        #endregion


    }
}