using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD.Domain.Helpers
{
    public static class Helper
    {
        public static void GetErrorNumberAndErrorMessage(Exception exp, ref string errorNumber, ref string errorMessage)
        {
            if (exp.Source.Equals("IBM.Data.DB2")) // IBM
            {
                string[] split = exp.Message.Split(new char[] { ' ' });

                for (int i = 0; i < split.Length; i++)
                {
                    if (split[i].StartsWith("SQL"))
                    {
                        errorNumber = split[i].Trim();
                        errorMessage = string.Join(" ", split.Skip(i + 1));
                        break;
                    }
                }
            }
            else // Oracle
            {
                string[] split = exp.Message.Split(new char[] { ':' }, 2);
                errorNumber = split[0].Trim();
                errorMessage = split[1].Trim();
            }
        }
    }
}
