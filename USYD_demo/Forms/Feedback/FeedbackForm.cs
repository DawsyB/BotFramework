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
    public class FeedbackForm
    {
        internal static async Task ResumeAfterFeedbackFormDialog(IDialogContext context, IAwaitable<FeedbackQuery> result)
        {
            var feedback = await result;
            //todo: Implement sending of email with the feedback notes
            await context.PostAsync($"{Constants.SubmitFeedbackText}");
            context.Done<object>(null);
        }

        internal static IForm<FeedbackQuery> BuildFeedbackForm()
        {
            return new FormBuilder<FeedbackQuery>()
                 .Field(nameof(FeedbackQuery.feedback))                 
                 .Build();
        }
    }
}