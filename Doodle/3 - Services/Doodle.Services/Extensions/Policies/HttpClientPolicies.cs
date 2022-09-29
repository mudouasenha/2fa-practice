using Polly;
using Polly.Retry;

namespace Doodle.Services.Extensions.Policies
{
    public static class HttpClientPolicies
    {
        public static AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry => Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode).RetryAsync(5);

        public static AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry => Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3));

        public static AsyncRetryPolicy<HttpResponseMessage> ExponentialBackOffHttpRetry => Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}