using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;

namespace USYD_demo.Dialogs
{
    [Serializable]
    public class QnADialog : IDialog<object>
    {
        private static bool IsQnASuccess = false;
        private static int MaxAttempts = 3;
        public async Task StartAsync(IDialogContext context)
        {
            
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            var responseString = String.Empty;
            var responseMsg = "";

            //De-serialize the response
            QnAMakerResult QnAresponse;

            // Send question to API QnA bot
            if (activity.Text.Length > 0)
            {
                var knowledgebaseId = ConfigurationManager.AppSettings["QnAId"].ToString(); // Use knowledge base id created.
                var qnamakerSubscriptionKey = ConfigurationManager.AppSettings["QnASubscriptionKey"].ToString(); ; //Use subscription key assigned to you.

                //Build the URI
                Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
                var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

                //Add the question as part of the body
                var postBody = $"{{\"question\": \"{activity.Text}\"}}";

                //Send the POST request
                using (WebClient client = new WebClient())
                {
                    //Set the encoding to UTF8
                    client.Encoding = System.Text.Encoding.UTF8;

                    //Add the subscription key header
                    client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                    client.Headers.Add("Content-Type", "application/json");
                    responseString = client.UploadString(builder.Uri, postBody);
                }

                try
                {
                    QnAresponse = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
                    responseMsg = QnAresponse.Answer.ToString();
                }
                catch
                {
                    throw new Exception("Unable to deserialize QnA Maker response string.");
                }
                // return our reply to the user 
                if (QnAresponse.Score > 0)
                {
                    MaxAttempts = 3;
                    IsQnASuccess = true;
                    context.ConversationData.SetValue<bool>("MaxedOutAttempts", false);
                    await context.PostAsync("We found the below information from Knowledge Base!");
                    await context.PostAsync(responseMsg);
                }
                else
                {
                    MaxAttempts--;
                    await context.PostAsync($"Unfortunately, we didn't find anything in Knowledge Base matching your query '{activity.Text}'. Please try rephrasing your query. Thanks");
                    if (MaxAttempts <= 0)
                    {
                        context.ConversationData.SetValue<bool>("MaxedOutAttempts", true);
                    }

                }
               
            }
            else
            {
                await context.PostAsync("Unfortunately, something has gone wrong.");
                //  context.Call(new mainDialog(), this.ResumeAfterMessageReceivedAsync);
                context.Wait(this.MessageReceivedAsync);
            }

        }

      
    }
}