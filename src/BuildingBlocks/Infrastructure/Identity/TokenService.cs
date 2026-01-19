using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Contracts.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;
using Shared.DTOs.Identity;

namespace Infrastructure.Identity;

public class TokenService : ITokenService
{
	private readonly JwtSettings _jwtSettings;
	public TokenService(JwtSettings jwtSettings)
	{
		_jwtSettings = jwtSettings;
	}
	public TokenResponse GetToken(TokenRequest request)
	{
		var token = GenerateJwt();
		var result = new TokenResponse(token);
		return result;
	}
	private string GenerateJwt() => GenerateEncryptedToken(GetSigningCredentials());
	private string GenerateEncryptedToken(SigningCredentials signingCredentials)
	{
		var token = new JwtSecurityToken(
			signingCredentials: signingCredentials,
			expires: DateTime.UtcNow.AddMinutes(30)
		);

		var tokenHander = new JwtSecurityTokenHandler();
		return tokenHander.WriteToken(token);
	}
	private SigningCredentials GetSigningCredentials()
	{
		byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
		return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
	}
}
