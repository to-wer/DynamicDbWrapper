using System;
using System.Collections.Generic;
using System.Text;

namespace ToWer.DbWrapper.Exceptions
{
    public class SqlCmdException : Exception
    {
        public int ErrNum { get; set; }
        public string ErrText { get; set; }

        public SqlCmdException()
            : base()
        {

        }

        public SqlCmdException(string message, int errNum, string errText)
            :base(message)
        {
            ErrNum = errNum;
            ErrText = errText;
        }
    }
}
