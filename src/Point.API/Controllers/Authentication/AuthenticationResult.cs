using Microsoft.AspNetCore.Identity;

namespace Point.API.Controllers.Authentication
{
    public class AuthenticationResult
    {
        public bool Result { get; set; }

        public IEnumerable<IdentityError> Errors { get; set; }

        public string Message { get; set; }

        public string Code { get; set; }
    }
}
