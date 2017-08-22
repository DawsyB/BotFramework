using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace USYD_demo
{
    [Serializable]
    public class POForms
    {
        #region PO LIST Display
        public IForm<POQuery> BuildPOListForm()
        {
            return new FormBuilder<POQuery>()
                 .Field(nameof(POQuery.PONumber), (state) => string.IsNullOrEmpty(state.PONumber))
                 .Field(nameof(POQuery.UniKey))
                 .Build();
        }

        internal async Task ResumeAfterPOListCallDialog(IDialogContext context, IAwaitable<POQuery> result)
        {
            var POfetchObj = await result;
            await context.PostAsync($"PO Number: {POfetchObj.PONumber},Account Nunber: {POfetchObj.AccountNumber}, UniKey: {POfetchObj.UniKey}");
            //todo: Implement integration here and return the results to the user
            await  this.confirmAction(context);
        }




        #endregion

        #region Common
        private async Task confirmAction(IDialogContext context)
        {
            PromptDialog.Confirm(context, this.ResumeAfterConfirmationPrompt, Constants.POConfirmPromptText);
        }

        private async Task ResumeAfterConfirmationPrompt(IDialogContext context, IAwaitable<bool> result)
        {
            var res = await result;
            if (res)
            {
                await context.PostAsync("Continuing session....");
                context.Done<object>(null);
            }
            else
            {
                PromptDialog.Confirm(context, this.ResumeAfterFeedbackPrompt, Constants.FeedbackPromptText);               
            }
        }

        private async Task ResumeAfterFeedbackPrompt(IDialogContext context, IAwaitable<bool> result)
        {
            var res = await result;
            if (res)
            {
                var FeedbackFormDialog = new FormDialog<FeedbackQuery>(new FeedbackQuery(), FeedbackForm.BuildFeedbackForm, FormOptions.PromptInStart);
                context.Call(FeedbackFormDialog, FeedbackForm.ResumeAfterFeedbackFormDialog);
            }
            else
            {
                //todo: Implement conversation signoff
                await context.PostAsync("Ending coversation!!");
                context.Done<object>(null);
            }
        }


        #endregion
    }


}