using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "Server"; 
        public const string AUDIENCE = "Cli"; 
        const string KEY = "32simvolov12345632simvolov123456";   
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
