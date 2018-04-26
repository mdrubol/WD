using System;
using System.Data;
using System.Linq;
using System.Web.Http;
using WD.DataAccess.Abstract;
using WD.DataAccess.Context;
using WD.DataAccess.Helpers;
using WD.DataAccess.Logger;

namespace WD.WebApi.Controllers
{
  
    public class DataTableController : ApiController
    {
        public ICommands ICommands { get; set; }
        public DataTable DtOne = new DataTable();
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
                DtOne = ICommands.ExecuteDataTable(input.TheSql.FirstOrDefault().CommandText, input.TheSql.FirstOrDefault().CommandType,input.TheSql.FirstOrDefault().Params);
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
            return Ok(DtOne);
        }
      

    }
}
