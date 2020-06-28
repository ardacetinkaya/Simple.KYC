namespace Simple.KYC.Components
{
    using Microsoft.AspNetCore.Components;
    using Simple.KYC.Data;
    using Simple.KYC.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Tweetinvi;

    public class AnalyseTweetsModalBase : ComponentBase,IDisposable
    {
        [Inject] AnalyseTweets AnalyseTweetsService { get; set; }

        protected bool IsVisible { get; set; }
        protected string Title { get; set; }
        protected List<UserTweet> Tweets { get; set; }
        protected override Task OnInitializedAsync()
        {
            AnalyseTweetsService.OnShow += ShowModal;
            AnalyseTweetsService.OnClose += CloseModal;
            return base.OnInitializedAsync();
        }
        public void Open(string title,long userId)
        {
            AnalyseTweetsService.Show(title,userId);
        }
        public void Close()
        {
            AnalyseTweetsService.Close();
        }

        protected void ShowModal(string title,List<UserTweet> userTweets)
        {
            Title = title;
            IsVisible = true;
            Tweets = userTweets;
            StateHasChanged();
        }
        protected void CloseModal()
        {
            IsVisible = false;
            Title = "";

            StateHasChanged();
        }
        public void Dispose()
        {
            AnalyseTweetsService.OnShow -= ShowModal;
            AnalyseTweetsService.OnClose -= CloseModal;
        }
    }
}
