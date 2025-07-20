using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;
using EStoreX.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EStoreX.Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token using the given user's information and the configuration settings.
        /// </summary>
        /// <param name="user">ApplicationUser object</param>
        /// <returns>AuthenticationResponse that includes token</returns>
        public AuthenticationSuccessResponse CreateJwtToken(ApplicationUser user)
        {
            // Create a DateTime object representing the token expiration time by adding the number of minutes specified in the configuration to the current UTC time.
            DateTime expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

            // Create an array of Claim objects representing the user's claims, such as their ID, name, email, etc.
            List<Claim> claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), //Subject (user id)
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //JWT unique ID
                 new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()), //Issued at (date and time of token generation)
                 new Claim(ClaimTypes.NameIdentifier, user.Email), //Unique name identifier of the user (Email)
                 new Claim(ClaimTypes.Name, user.UserName), //Name of the user
                 new Claim(ClaimTypes.Email, user.Email) //Email of the user
             };

            var audienceClaims = _configuration.GetSection("Jwt:Audiences").Get<string[]>();
            foreach (var aud in audienceClaims)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Aud, aud));
            }


            // Create a SymmetricSecurityKey object using the key specified in the configuration.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Create a SigningCredentials object with the security key and the HMACSHA256 algorithm.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create a JwtSecurityToken object with the given issuer, audience, claims, expiration, and signing credentials.
            JwtSecurityToken tokenGenerator = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

            // Create a JwtSecurityTokenHandler object and use it to write the token as a string.
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);

            // Create and return an AuthenticationResponse object containing the token, user email, user name, and token expiration time.
            return new AuthenticationSuccessResponse()
            {
                Token = token,
                Email = user.Email,
                UserName = user.UserName,
                Expiration = expiration,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["RefreshToken:EXPIRATION_MINUTES"])),
            };
        }

        /// <summary>
        /// Create Refresh Token (base 64 string of random number)
        /// </summary>
        /// <returns>Refresh Token</returns>
        private string GenerateRefreshToken()
        {
            Byte[] bytes = new byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// Get ClaimsPrincipal from JWT token
        /// </summary>
        /// <param name="token">Access Token</param>
        /// <returns></returns>
        public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudiences = _configuration.GetSection("Jwt:Audiences").Get<List<string>>(),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal claimsPrincipal =
                jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return claimsPrincipal;
        }
    }
}
