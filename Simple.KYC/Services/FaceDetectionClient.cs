namespace Simple.KYC.Services
{
    using Microsoft.Azure.CognitiveServices.Vision.Face;
    using Microsoft.Extensions.Configuration;
    using Simple.KYC.Data;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class FaceDetectionClient
    {
        private readonly IConfiguration _configuration;
        private FaceClient _faceClient;
        public FaceDetectionClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(_configuration["FACE_KEY"]));
            _faceClient.Endpoint = _configuration["FACE_ENDPOINT"];
        }
        public async Task<IEnumerable<string>> DetectFace(Stream faceStream)
        {
            List<string> result = new List<string>();
            try
            {
                if (faceStream != null)
                {
                    var face = _faceClient.Face.DetectWithStreamAsync(faceStream);
                    face.Wait();
                    var faces = face.Result;
                    if (faces.Any())
                    {
                        result.AddRange(faces.Select(f => f.FaceId.ToString()).ToList());
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            return result;
        }

        public  IEnumerable<string> DetectFace(string url)
        {

            List<string> result = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var face = _faceClient.Face.DetectWithUrlAsync(url);
                    face.Wait();
                    var faces = face.Result;

                    if (faces.Any())
                    {
                        result.AddRange(faces.Select(f => f.FaceId.ToString()).ToList());
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public PhotoVerificationResult VerifyFaces(Guid faceId1, Guid faceId2)
        {
            try
            {
                if (faceId1 != Guid.Empty && faceId2 != Guid.Empty)
                {
                    var face = _faceClient.Face.VerifyFaceToFaceAsync(faceId1, faceId2);
                    face.Wait();
                    var result = face.Result;

                    return new PhotoVerificationResult
                    {
                        IsIdentical = result.IsIdentical,
                        Confidence = result.Confidence
                    };

                }
            }
            catch (Exception ex)
            {

            }

            return new PhotoVerificationResult { IsIdentical = false, Confidence = 0.0 };
        }
    }
}
