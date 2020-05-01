using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Enxaquecapp.Data;
using Enxaquecapp.Domain;
using Enxaquecapp.WebApi.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace Enxaquecapp.WebApi.Security
{
    public class TokenProvider
    {
        TokenSettings _settings;
        private GenericRepository<User> _usersRepository;

        public TokenProvider(
            TokenSettings settings,
            GenericRepository<User> studentsRepository)
        {
            _settings = settings;
            _usersRepository = studentsRepository;
        }

        public async Task<TokenViewModel> GenerateTokenAsync(string email, string password)
        {
            var user = _usersRepository.GetQueryable().SingleOrDefault(s => s.Email == email);

            if (user == null)
                throw new ArgumentException($"E-mail {email} não cadastrado");

            if (!user.ValidatePassword(password))
                throw new ArgumentException("senha inválida");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_settings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(DefaultClaims.Username, user.Email),
                    new Claim(DefaultClaims.UserId, user.Id.ToString())
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddYears(7),
                Audience = _settings.Audience,
                Issuer = _settings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var viewModel = new TokenViewModel
            {
                Token = tokenHandler.WriteToken(token)
            };

            return viewModel;
        }
    }
}