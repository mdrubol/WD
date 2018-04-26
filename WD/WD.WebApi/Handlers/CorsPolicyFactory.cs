using System;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using WD.WebApi.Attributes;

namespace WD.WebApi.Handlers
{
    public class CorsPolicyFactory : ICorsPolicyProviderFactory
    {
        public ICorsPolicyProvider GetCorsPolicyProvider(System.Net.Http.HttpRequestMessage request)
        {
            return new MyCorsPolicyProviderAttribute(); 
        }
    } 
}