using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_LogicLayer
{
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception inner): base(message, inner)  
        {
            
        }
    }
}
