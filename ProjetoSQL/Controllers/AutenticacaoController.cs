using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjetoSQL.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoSQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly ConfiguracoesJWT ConfiguracoesJWT;
        public AutenticacaoController(IOptions<ConfiguracoesJWT> opcoes)
        {
            ConfiguracoesJWT = opcoes.Value;
        }
        [HttpGet("Gerar token de Gerente")]
        public IActionResult ObterToken()
        {
            var token = GerarToken();
           
            var retorno = new
            {
                Token = token
            };

            return Ok(token);
        }
        //SECRETARIA
        [HttpGet("Gerar token de Secretaria")]
        public IActionResult ObterToken2()
        {
            var token2 = GerarToken2();

            var retorno2 = new
            {
                Token2 = token2
            };

            return Ok(token2);
        }
        private string GerarToken2()
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Nome", "Bootcamp"));
            claims.Add(new Claim(ClaimTypes.Role, "Secretaria"));


            var handler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfiguracoesJWT.Segredo)), SecurityAlgorithms.HmacSha256Signature),
                Audience = "https://localhost:5001",
                Issuer = "Bootcamp2022",
                Subject = new ClaimsIdentity(claims),
            };

            SecurityToken token2 = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token2);

        }
        //fim
        private string GerarToken()
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Nome", "Bootcamp"));
            claims.Add(new Claim(ClaimTypes.Role, "Gerente"));


            var handler = new JwtSecurityTokenHandler();
            
            var tokenDescriptor = new SecurityTokenDescriptor { 
                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("QWERTYUIOP:,123456789")), SecurityAlgorithms.HmacSha256Signature),
                Audience = "https://localhost:5001",
                Issuer = "Bootcamp2022",
                Subject = new ClaimsIdentity(claims),
            };

           SecurityToken token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);

        }
    }
}
