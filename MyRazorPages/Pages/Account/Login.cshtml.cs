using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyRazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        public IActionResult OnPost(string username, string password)
        {
            if ((username == "admin" && password == "password") || (username == "user" && password == "password"))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("KeyniN@42uha4%(*HFANwsWHNf2ygujtyfg35");
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };

                if (username == "admin")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                }
                else if (username == "user")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                Response.Cookies.Append("AuthToken", tokenString);

                return RedirectToPage("/Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
