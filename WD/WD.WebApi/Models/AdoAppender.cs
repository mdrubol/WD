using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using WD.DataAccess.Enums;
using WD.DataAccess.Logger;

namespace WD.WebApi.Models
{
    public class AdoAppender :LogAppender
    {

        public AdoAppender():base() { 
         
            DoActiveOptions +=AdoAppender_DoActiveOptions;
            DoLogging += AdoAppender_DoLogging;
            DoLoggings += AdoAppender_DoLoggings;
        }

        private void AdoAppender_DoLoggings(log4net.Core.LoggingEvent[] loggingEvents)
        {
           // throw new NotImplementedException();
        }

        private void AdoAppender_DoLogging(log4net.Core.LoggingEvent logginEvent)
        {
           // throw new NotImplementedException();
            string message = logginEvent.RenderedMessage;
            if (RegexStatement.Match(message).Success)
            { 

                //SELECT/INSERT /UPDATE / DELETE/
            }
        }

        private void AdoAppender_DoClose()
        {
            //throw new NotImplementedException();
        }

        private void AdoAppender_DoActiveOptions()
        {
            //throw new NotImplementedException();
        }

    }
}