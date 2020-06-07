using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enxaquecapp.Data;
using Enxaquecapp.Domain;
using Enxaquecapp.WebApi.Extensions;
using Enxaquecapp.WebApi.InputModels;
using Enxaquecapp.WebApi.Security;
using Enxaquecapp.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enxaquecapp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiControllerBase
    {
        private GenericRepository<User> _usersRepository;
        private GenericRepository<Cause> _causesRepository;
        private GenericRepository<Relief> _reliefsRepository;
        private GenericRepository<Local> _localsRepository;

        public UsersController(
            GenericRepository<User> usersRepository,
            GenericRepository<Cause> causesRepository,
            GenericRepository<Relief> reliefsRepository,
            GenericRepository<Local> localsRepository)
        {
            _usersRepository = usersRepository;
            _causesRepository = causesRepository;
            _reliefsRepository = reliefsRepository;
            _localsRepository = localsRepository;
        }

        /// <summary>
        /// Get Current User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public Task<ActionResult<UserViewModel>> GetAsync()
            => ExecuteAsync<UserViewModel>(async () =>
            {
                var userId = User.UserId();
                var user = await _usersRepository.GetByIdAsync(userId);

                if (user == null)
                    return NotFound();

                return Ok((UserViewModel) user);

            });

        /// <summary>
        /// Create a User
        /// </summary>
        /// <param name="inputModel">User information</param>
        /// <returns></returns>
        [HttpPost]
        public Task<ActionResult<TokenViewModel>> PostAsync([FromServices] TokenProvider provider, [FromBody] UserInputModel inputModel)
            => ExecuteAsync<TokenViewModel>(async () =>
            {
                var sex = Gender.NotDisclosed;

                if (inputModel.Gender.ToUpper() == "Masculino".ToUpper())
                    sex = Gender.Male;

                if (inputModel.Gender.ToUpper() == "Feminino".ToUpper())
                    sex = Gender.Female;

                var user = new User(inputModel.Name, inputModel.Email, inputModel.Password, inputModel.BirthDate, sex);;
                await _usersRepository.AddAsync(user);
                await AddDefaultOptionsAsync(user);

                var result = await provider.GenerateTokenAsync(inputModel.Email, inputModel.Password);

                return Ok((TokenViewModel) result);
            });

        /// <summary>
        /// Update an User
        /// </summary>
        /// <param name="id">Id of the User to be updated</param>
        /// <param name="inputModel">User information</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        public Task<ActionResult<UserViewModel>> PatchAsync(Guid id, [FromBody] UserUpdateInputModel inputModel)
            => ExecuteAsync<UserViewModel>(async () =>
            {
                var userId = User.UserId();
                var user = await _usersRepository.GetByIdAsync(id);

                if (user == null)
                    return NotFound();

                if (inputModel.Name != null)
                    user.SetName(inputModel.Name);

                if (inputModel.Email != null)
                    user.SetEmail(inputModel.Email);

                if (inputModel.Password != null)
                    user.SetPassword(inputModel.Password);

                if (inputModel.BirthDate != null)
                    user.SetBirthDate(inputModel.BirthDate.Value);

                if (inputModel.Sex != null)
                {
                    var sex = Gender.NotDisclosed;
                    Enum.TryParse<Gender>(inputModel.Sex, true, out sex);
                    user.Gender = sex;
                }

                await _usersRepository.UpdateAsync(user);

                return Ok((UserViewModel) user);
            });

        /// <summary>
        /// Delete an User
        /// </summary>
        /// <param name="id">Id of the User</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public Task<ActionResult> DeleteAsync(Guid id)
            => ExecuteAsync(async () =>
            {
                var userId = User.UserId();
                var user = await _usersRepository.GetByIdAsync(id);

                if (user == null)
                    return NotFound();

                await _usersRepository.DeleteAsync(user);

                return Ok();
            });

        private async Task AddDefaultOptionsAsync(User user)
        {
            var reliefs = new List<Relief>
            {
                new Relief(user, "Relief 1", null),
                new Relief(user, "Relief 2", null)
            };

            var causes = new List<Cause>
            {
                new Cause(user, "Cause 1", null),
                new Cause(user, "Cause 2", null)
            };

            var locals = new List<Local>
            {
                new Local(user, "Local 1", null),
                new Local(user, "Local 2", null)
            };

            await _reliefsRepository.AddRangeAsync(reliefs);
            await _causesRepository.AddRangeAsync(causes);
            await _localsRepository.AddRangeAsync(locals);
        }
    }
}