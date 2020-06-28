namespace Simple.KYC.Services
{
    using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Rest;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    public class TextAnalyseClient
    {
        private readonly IConfiguration _configuration;
        private readonly TextAnalyticsClient _textAnalyticsClient;
        public TextAnalyseClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _textAnalyticsClient = new TextAnalyticsClient(new ApiKeyServiceClientCredentials(_configuration["TEXT_KEY"]));
            _textAnalyticsClient.Endpoint = _configuration["TEXT_ENDPOINT"];
        }

        public List<string> AnalyseKeyPhrases(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var keyPhraseAnalyse =_textAnalyticsClient.KeyPhrasesAsync(text);
                keyPhraseAnalyse.Wait();

                return keyPhraseAnalyse.Result.KeyPhrases.ToList();
            }

            return new List<string>();
        }

        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            private readonly string apiKey;

            public ApiKeyServiceClientCredentials(string apiKey)
            {
                this.apiKey = apiKey;
            }

            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (request == null)
                {
                    throw new ArgumentNullException("request");
                }
                request.Headers.Add("Ocp-Apim-Subscription-Key", this.apiKey);
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }
    }
}
