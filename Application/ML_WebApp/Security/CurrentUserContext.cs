using ML_LogicLayer.Security;
using System.Security.Claims;

public class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier);

            return int.Parse(claim.Value);
        }
    }

    public string Role
    {
        get
        {
            return _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.Role)
                .Value;
        }
    }

   public bool IsActive
    { get { return true; } }
}
