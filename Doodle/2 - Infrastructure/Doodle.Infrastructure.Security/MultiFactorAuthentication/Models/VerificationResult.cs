using Newtonsoft.Json;

namespace Doodle.Infrastructure.Security.MultiFactorAuthentication.Models
{
    public class VerificationResult
    {
        public VerificationResult(string sid)
        {
            Sid = sid;
            IsValid = true;
        }

        public VerificationResult(object binding)
        {
            Binding = JsonConvert.DeserializeObject<Binding>(binding.ToString());
            IsValid = true;
        }

        public VerificationResult(List<string> errors)
        {
            Errors = errors;
            IsValid = false;
        }

        public string Message { get; set; }

        public bool IsValid { get; set; }

        public Binding Binding { get; set; }

        public string Sid { get; set; }

        public List<string> Errors { get; set; }
    }

    public class Binding
    {
        public string Secret { get; set; }

        public string Uri { get; set; }
    }
}