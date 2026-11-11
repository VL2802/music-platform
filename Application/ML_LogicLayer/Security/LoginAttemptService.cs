using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_LogicLayer.Security
{
    public class LoginAttemptService
    {
        private const int MaxAttempts = 5;

        public bool CanAttemptLogin(int failedAttempts)
        {
            return failedAttempts < MaxAttempts;
        }
    }

}
