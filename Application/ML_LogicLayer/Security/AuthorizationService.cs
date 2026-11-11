using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_LogicLayer.Security
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICurrentUserContext _currentUserContext;

        public AuthorizationService(ICurrentUserContext currentUserContext)
        {
            _currentUserContext = currentUserContext;
        }

        public void EnsureAuthenticated()
        {
            if (_currentUserContext.UserId <= 0)
                throw new UnauthorizedAccessException();
        }

        public void EnsureArtist()
        {
            if (_currentUserContext.Role != "Artist")
                throw new UnauthorizedAccessException();
        }

        public void EnsureListener()
        {
            if (_currentUserContext.Role != "Listener")
                throw new UnauthorizedAccessException();
        }

        public void EnsureActiveAccount()
        {
            if (!_currentUserContext.IsActive)
                throw new UnauthorizedAccessException();
        }
    }
}
