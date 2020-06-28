
namespace Simple.KYC.Services
{
    using Simple.KYC.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tweetinvi;
    using Tweetinvi.Parameters;

    public class AnalyseTweets
    {
        public event Action<string, List<UserTweet>> OnShow;
        public event Action OnClose;
        private readonly TextAnalyseClient _client;

        public AnalyseTweets(TextAnalyseClient client)
        {
            _client = client;
        }

        public void Show(string title, long userId)
        {
            List<UserTweet> _tweets = new List<UserTweet>();
            var user = User.GetUserFromId(userId);
            var userTimeline = user.GetUserTimeline(new UserTimelineParameters
            {
                ExcludeReplies = true,
                IncludeRTS = false,
                MaximumNumberOfTweetsToRetrieve = 10
            });
            foreach (var tweet in userTimeline)
            {
                if (!tweet.IsRetweet && tweet.InReplyToScreenName == null)
                {

                    var keys = _client.AnalyseKeyPhrases(tweet.Text);
                   
                    _tweets.Add(new UserTweet
                    {
                        Text = string.Join(" - ", keys),
                        URL = tweet.Url,
                        Date = tweet.CreatedAt
                    });
                }
            }
            OnShow?.Invoke(title, _tweets);
        }

        public void Close()
        {
            OnClose?.Invoke();
        }
    }
}
