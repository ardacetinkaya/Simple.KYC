using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Simple.KYC.Components;
using Simple.KYC.Data;
using Simple.KYC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;

namespace Simple.KYC.Pages
{
    public class IndexBase : ComponentBase
    {
        public Identity Identity = new Identity();
        public List<TwitterUser> Users;
        public string FaceId;
        public bool IsSearchInProgress;
        public bool IsPhotoUploadInProgress;
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IConfiguration Configuration { get; set; }
        [Inject] protected AnalyseTweets AnalyseTweetsService { get; set; }
        [Inject] protected FaceDetectionClient FaceDetectionClient { get; set; }

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }
        protected async Task Upload()
        {
            IsPhotoUploadInProgress = true;
            Auth.SetUserCredentials(Configuration["TWITTER_CONSUMER_KEY"], Configuration["TWITTER_CONSUMER_SECRET"],
                Configuration["TWITTER_ACCESS_TOKEN"], Configuration["TWITTER_ACCESS_TOKEN_SECRET"]);

            var uploadResult = await JSRuntime.InvokeAsync<bool>("UploadPhoto", DotNetObjectReference.Create(this));
        }

        protected async Task Submit()
        {
            if (!string.IsNullOrEmpty(Identity.Name))
            {
                IsSearchInProgress = true;
                Auth.SetUserCredentials(Configuration["TWITTER_CONSUMER_KEY"], Configuration["TWITTER_CONSUMER_SECRET"],
                    Configuration["TWITTER_ACCESS_TOKEN"], Configuration["TWITTER_ACCESS_TOKEN_SECRET"]);

                var searchResults = await SearchAsync.SearchUsers(Identity.Name);

                if (searchResults != null && searchResults.Any())
                {
                    Users = new List<TwitterUser>();
                    foreach (var user in searchResults)
                    {
                        var faces = FaceDetectionClient.DetectFace(user.ProfileImageUrlFullSize);
                        if (faces.Any())
                        {

                            var isIdentical = false;
                            var confidence = 0.0;
                            if (!string.IsNullOrEmpty(FaceId))
                            {
                                var face1 = new Guid(FaceId);
                                var face2 = new Guid(faces.FirstOrDefault());

                                var verificationResult = FaceDetectionClient.VerifyFaces(face1, face2);
                                isIdentical = verificationResult.IsIdentical;
                                confidence = verificationResult.Confidence;
                            }

                       
                            Users.Add(new TwitterUser()
                            {
                                Id = user.Id,
                                Name = user.Name,
                                ScreenName = user.ScreenName,
                                Info = user.Description,
                                IsVerified = user.Verified,
                                ProfilePhoto = user.ProfileImageUrlFullSize,
                                IsIdentical = isIdentical,
                                Confidence = confidence
                            });

                        }
                    }
                }
                IsSearchInProgress = false;
            }

            StateHasChanged();

        }

        protected void AnaylzeTweets(long userId)
        {
            AnalyseTweetsService.Show("Tweets",userId);
            
        }
        [JSInvokable]
        public bool SetFaceID(List<string> faceIDs)
        {
            //Photo may have more than one face
            //Just take one of them...
            if (faceIDs.Any())
            {
                FaceId = faceIDs.FirstOrDefault();
            }
            else
            {
                FaceId = null;
            }

            IsPhotoUploadInProgress = false;
            StateHasChanged();
            return true;
        }
    }
}
