using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.SpellCheck;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotAppWithSpellCheck.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            //await context.PostAsync($"You sent {activity.Text} which was {length} characters");

            // return api results to user
            await context.PostAsync($"For {activity.Text} input, Spell Check API returned => \n {await SpellCheckAsync(activity.Text)}");

            context.Wait(MessageReceivedAsync);
        }

        private async Task<string> SpellCheckAsync(string inputText)
        {
            var client = new SpellCheckClient(new ApiKeyServiceClientCredentials(ConfigurationManager.AppSettings["BingSpellCheckKey"]));

            //var response = await client.SpellCheckerWithHttpMessagesAsync(inputText);
            /*
             * For SpellCheck mode, The default is Proof. 1) Proof—Finds most spelling and grammar mistakes. 
             * 2) Spell—Finds most spelling mistakes but does not find some of the grammar errors that Proof
             * catches (for example, capitalization and repeated words). Possible values
             * include: 'proof', 'spell'
             * Ref SDK - https://github.com/Azure/azure-sdk-for-net/blob/psSdkJson6/src/SDKs/CognitiveServices/dataPlane/Language/SpellCheck/BingSpellCheck/Generated/SpellCheckClient.cs
             * */
            var response = await client.SpellCheckerWithHttpMessagesAsync(text: inputText,
                                                            mode: "spell", acceptLanguage: "en-US");

            //Get the service task result
            var result = response.Response;

            var spellcheckContent = await result.Content.ReadAsStringAsync();

            return spellcheckContent;
        }
    }
}