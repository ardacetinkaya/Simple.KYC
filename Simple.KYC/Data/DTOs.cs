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
        public double Confidence { get; internal set; }
        public bool IsIdentical { get; internal set; }
        public string Info { get; internal set; }
    }

    public class PhotoVerificationResult
    {
        public bool IsIdentical { get; set; }
        public double Confidence { get; set; }
    }

}
