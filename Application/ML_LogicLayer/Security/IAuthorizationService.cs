using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_LogicLayer.Security
{
    public interface IAuthorizationService
    {
        void EnsureAuthenticated();
        void EnsureArtist();
        void EnsureListener();
        void EnsureActiveAccount();
    }
}
