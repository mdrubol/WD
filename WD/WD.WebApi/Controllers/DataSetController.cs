using System;
using System.Data;
using System.Linq;
using System.Web.Http;
using WD.DataAccess.Abstract;
using WD.DataAccess.Helpers;
using WD.DataAccess.Context;
using WD.DataAccess.Logger;
namespace WD.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
  
    public class DataSetController : ApiController
    {

        public ICommands ICommands { get; set; }
        public DataSet Ds = new DataSet();
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
                if (string.IsNullOrEmpty(input.AuthenticationToken))
                {
                    throw new Exception("token cannot be empty");
                }
                else if (System.Configuration.ConfigurationManager.AppSettings[WebSecurityUtility.Decrypt(input.AuthenticationToken, true)] == "true")
                {
                    ICommands = new DbContext(input.Connect).ICommands;
                    Ds = ICommands.ExecuteDataSet(input.TheSql);
                }
                else {

                    throw new Exception("invalid token");
                }
               
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
            return Ok(Ds);
        }
        

        

    }
}
