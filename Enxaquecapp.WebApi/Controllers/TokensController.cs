using System;
using System.Threading.Tasks;
using Enxaquecapp.WebApi.InputModels;
using Enxaquecapp.WebApi.Security;
using Enxaquecapp.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Enxaquecapp.WebApi.Controllers
{
    /// <summary>
    /// endpoints for token generation
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TokensController : ControllerBase
    {
        private TokenProvider _provider;

        /// <summary>
        /// default controller userd by the service provider
        /// </summary>
        public TokensController(TokenProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Generates a jwt token
        /// </summary>
        /// <param name="inputModel">Input model of the token</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TokenViewModel>> PostAsync([FromBody] TokenInputModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            try
            {
                var result = await _provider.GenerateTokenAsync(inputModel.Email, inputModel.Password);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}