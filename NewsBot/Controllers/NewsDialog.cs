
using BingSearchHelper;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NewsBot
{
    [LuisModel("<INSERT LUIS MODEL GUID>", "<INSERT LUIS SUBSCRIPTION GUID>")]
    [Serializable]
    public class NewsDialog : LuisDialog<object>
    {
        private static string BingSearchApiKey = "<INSERT BING SEARCH KEY>";

        [LuisIntent("NewsSubject")]
        public async Task NewsSubject(IDialogContext context, LuisResult result)
        {
            string subject = result.Entities[0].Entity;

            //store this subject for the conversation
            context.ConversationData.SetValue<string>("LastSubject", subject);

            var newsResult = await GetNewsResult(subject);
            if (newsResult != null)
            {
                await context.PostAsync(string.Format("{0} was found in the news", subject));

                foreach (var item in newsResult)
                {
                    string news = string.Format("[{0}]({1})", item.Title, item.Url);
                    await context.PostAsync(news);
                }
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("WhatsHappening")]
        public async Task NewsSearch(IDialogContext context, LuisResult result)
        {
            string subject = "";

            //store this subject for the conversation
            context.ConversationData.SetValue<string>("LastSubject", subject);
            var newsResult = await GetNewsResult(subject);
            if (newsResult != null)
            {
                foreach (var item in newsResult)
                {
                    string news = string.Format("[{0}]({1})", item.Title, item.Url);
                    await context.PostAsync(news);
                }
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("RepeatLastSubject")]
        public async Task RepeatLastSubject(IDialogContext context, LuisResult result)
        {
            string strRet = string.Empty;
            string subject = string.Empty;
            if (!context.ConversationData.TryGetValue("LastSubject", out subject))
            {
                strRet = "I don't have a previous subject to look up!";
            }
            else
            {
                await context.PostAsync(string.Format("The Previously returned headline result for {0} is below", subject));

                var searchResult = await GetNewsResult(subject);
                if (searchResult != null)
                {
                    strRet = searchResult.FirstOrDefault().Title;
                }
            }
            await context.PostAsync(strRet);
            context.Wait(MessageReceived);
        }


        [LuisIntent("None")]
        public async Task NoneHandler(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry, I don't understand");
            context.Wait(MessageReceived);
        }

        private async Task<IEnumerable<NewsArticle>> GetNewsResult(string subject)
        {
            BingSearchHelper.BingSearchHelper.SearchApiKey = BingSearchApiKey;
            var newsResult = await BingSearchHelper.BingSearchHelper.GetNewsSearchResults(subject, 5);
            return newsResult;
        }
    }
}
