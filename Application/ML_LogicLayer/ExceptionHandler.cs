using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ML_LogicLayer
{
    public static class ExceptionHandler
    {
        public static string Handle(Exception ex)
        {
            return ex switch
            {
                DataAccessException => "Oops! Something went wrong accessing the database. Please try again later.",
                ArgumentNullException argNull => "Missing input: " + argNull.Message,
                ArgumentException arg => "Invalid data: " + arg.Message,
                InvalidOperationException iOp => iOp.Message,
                _ => "An unexpected error occurred. Please contact support if the problem persists."
            };
        }
    }
}
