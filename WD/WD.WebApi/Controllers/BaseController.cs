using System;
using System.Linq;
using System.Web.Http;
using WD.DataAccess.Abstract;

namespace WD.WebApi.Controllers
{
   
    public class BaseController : ApiController
    {
        public readonly ICommands ICommands;
        public BaseController() { 
        
        }

    }
}