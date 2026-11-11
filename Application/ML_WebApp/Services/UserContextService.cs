using ML_LogicLayer.Services;
using System.Security.Claims;

namespace ML_WebApp.Services
{
    public class UserContextService
    {
        private readonly UserService _userService;

        public UserContextService(UserService userService)
        {
            _userService = userService;
        }

        public int GetCurrentUserId(ClaimsPrincipal user)
        {
            string? email = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var foundUser = _userService.FindUserByEmail(email);
            return foundUser?.GetId() ?? throw new ArgumentException("User  not found.");
        }

    }
}
