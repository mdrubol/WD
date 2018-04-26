using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace WD.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class MyCorsPolicyProviderAttribute : Attribute, ICorsPolicyProvider
    {
        private readonly CorsPolicy policy;

        public MyCorsPolicyProviderAttribute()
        {
            // Create a CORS policy.
            policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true,
                AllowAnyOrigin = true,
                SupportsCredentials=true,
                 
            };
           
        }
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(policy);
        }
    }
}