using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using assignment.ModelViews;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class AuthController : Controller {
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IConfiguration _configuration;

  public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration) {
    _userManager = userManager;
    _configuration = configuration;
  }

  [Route("register")]
  [HttpPost]
  public async Task<ActionResult> InsertUser([FromBody] RegisterViewModel model) {
    var user = new ApplicationUser
    {
      Email = model.Email,
      UserName = model.Username,
      LastName = model.LastName,
      FirstName = model.FirstName,
      Country = model.Country,
      MobileNumber = model.MobileNumber,
      SecurityStamp = Guid.NewGuid().ToString()
    };
    var result = await _userManager.CreateAsync(user, model.Password);
    if (result.Succeeded) {
      await _userManager.AddToRoleAsync(user, "Member");
    }
    return Ok(new {Username = user.UserName});
  }

  [Route("login")]
  [HttpPost]
  public async Task<ActionResult> Login([FromBody] LoginViewModel model) {
    var user = await _userManager.FindByNameAsync(model.Username);
    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password)) {


            ////////////////!!!!!!
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };
            if (user.UserName == "a")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Member"));
            }
            ////////////////!!!!!!


            var signinKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

      int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

      var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Site"],
        audience: _configuration["Jwt:Site"],
        expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
        signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
        claims: claims /////////++++++++++++++
      );



            return Ok(
        new
        {
          token = new JwtSecurityTokenHandler().WriteToken(token),
          username = user.UserName,
          email = user.Email,
          firstname = user.FirstName,
          lastname = user.LastName,
          country = user.Country,
          mobilenumber = user.MobileNumber
        });
    }
    return Unauthorized();
  }
}
