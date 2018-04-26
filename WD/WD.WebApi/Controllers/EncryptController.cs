using System;
using System.Linq;
using System.Web.Http;
using WD.DataAccess.Helpers;
using WD.DataAccess.Logger;

namespace WD.WebApi.Controllers
{
    public class EncryptController : ApiController
    {
        [AcceptVerbs("Get")]
        public IHttpActionResult Get(string conString)
        {
            string result = "";
            try
            {
                result = WebSecurityUtility.Encrypt(conString, true);
            }
            catch(Exception exc) {

                ILogger.Error(exc);
                return BadRequest(exc.Message.ToString());
            }
            return Ok(result);
        }
       
    }
}