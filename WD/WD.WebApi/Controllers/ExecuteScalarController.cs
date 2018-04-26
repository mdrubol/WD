using System;
using System.Linq;
using System.Web.Http;
using WD.DataAccess.Abstract;
using WD.DataAccess.Helpers;
using WD.DataAccess.Context;
using WD.DataAccess.Logger;
namespace WD.WebApi.Controllers
{
    public class ExecuteScalarController : ApiController
    {
        public ICommands ICommands { get; set; }
        public object Result = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">Object of Entity</param>
        /// <returns></returns>
        [AcceptVerbs("Post")]
        public IHttpActionResult Post(Wrapper input)
        {
            try
            {
                ICommands = new DbContext(input.Connect).ICommands;
                Result = ICommands.ExecuteScalar(input.TheSql);
            }
            catch (Exception exc)
            {
                ILogger.Error(exc);
                return BadRequest(exc.Message.ToString());
            }
            finally
            {
                ICommands = null;
            }
            return Ok(Result);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            base.Dispose(disposing);
        }
    }
}
