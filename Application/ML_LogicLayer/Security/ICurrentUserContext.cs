using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_LogicLayer.Security
{
    public interface ICurrentUserContext
    {
        int UserId { get; }
        string Role { get; }
        bool IsActive { get; }

    }
}
