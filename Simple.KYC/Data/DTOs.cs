using System;
using System.ComponentModel.DataAnnotations;

namespace Simple.KYC.Data
{
    public class Identity
    {
        [Required]
        public string Name { get; set; }
    }

    public class TwitterUser
    {
        public string ScreenName { get; set; }
        public string Name { get; set; }
        public string ProfilePhoto { get; set; }
        public bool IsVerified { get; set; }
        public double Confidence { get; set; }
        public bool IsIdentical { get; set; }
        public string Info { get; set; }
        public long Id { get; internal set; }
    }

    public class UserTweet
    {
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string URL { get; set; }
    }

    public class PhotoVerificationResult
    {
        public bool IsIdentical { get; set; }
        public double Confidence { get; set; }
    }

}
