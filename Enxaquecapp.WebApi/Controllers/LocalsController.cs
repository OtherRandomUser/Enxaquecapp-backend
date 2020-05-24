using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enxaquecapp.Data;
using Enxaquecapp.Domain;
using Enxaquecapp.WebApi.Extensions;
using Enxaquecapp.WebApi.InputModels;
using Enxaquecapp.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enxaquecapp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalsController : ApiControllerBase
    {
        private GenericRepository<Local> _localsRepository;
        private GenericRepository<User> _usersRepository;

        public LocalsController(
            GenericRepository<Local> localsRepository,
            GenericRepository<User> usersRepository)
        {
            _localsRepository = localsRepository;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Get all Locals registered for the logged user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public Task<ActionResult<IEnumerable<LocalViewModel>>> GetAsync()
            => ExecuteAsync<IEnumerable<LocalViewModel>>(async () =>
            {
                var userId = User.UserId();
                var locals = await Task.Run(() =>
                    _localsRepository
                        .GetQueryable()
                        .Where(c => c.UserId == userId)
                        .AsEnumerable()
                        .Select(c => (LocalViewModel) c));

                if (!locals.Any())
                    return NoContent();

                return Ok(locals);
            });

        /// <summary>
        /// Get a Local via id
        /// </summary>
        /// <param name="id">id of the cause</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public Task<ActionResult<LocalViewModel>> GetAsync(Guid id)
            => ExecuteAsync<LocalViewModel>(async () =>
            {
                var userId = User.UserId();
                var local = await _localsRepository.GetByIdAsync(id);

                if (local.UserId != userId)
                    return BadRequest();

                if (local == null)
                    return NotFound();

                if (local.UserId != userId)
                    return BadRequest();

                return Ok((LocalViewModel) local);
            });

        /// <summary>
        /// Create a New Local
        /// </summary>
        /// <param name="inputModel">Local information</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public Task<ActionResult<LocalViewModel>> PostAsync([FromBody] LocalInputModel inputModel)
            => ExecuteAsync<LocalViewModel>(async () =>
            {
                var userId = User.UserId();
                var user = await _usersRepository.GetByIdAsync(userId);
                var local = new Local(user, inputModel.Description, inputModel.Icon);

                await _localsRepository.AddAsync(local);

                return Ok((LocalViewModel) local);
            });

        /// <summary>
        /// Update a Local
        /// </summary>
        /// <param name="id">Id of the Local to be updated</param>
        /// <param name="inputModel">Local information</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public Task<ActionResult<LocalViewModel>> PutAsync(Guid id, [FromBody] LocalInputModel inputModel)
            => ExecuteAsync<LocalViewModel>(async () =>
            {
                var userId = User.UserId();
                var local = await _localsRepository.GetByIdAsync(id);

                if (local.UserId != userId)
                    return BadRequest();

                local.SetDescription(inputModel.Description);
                local.Icon = inputModel.Icon;

                await _localsRepository.UpdateAsync(local);

                return Ok((LocalViewModel) local);
            });

        /// <summary>
        /// Delete a Local
        /// </summary>
        /// <param name="id">Id of the local</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public Task<ActionResult> DeleteAsync(Guid id)
            => ExecuteAsync(async () =>
            {
                var userId = User.UserId();
                var local = await _localsRepository.GetByIdAsync(id);

                if (local.UserId != userId)
                    return BadRequest();

                await _localsRepository.DeleteAsync(local);

                return Ok();
            });
    }
}