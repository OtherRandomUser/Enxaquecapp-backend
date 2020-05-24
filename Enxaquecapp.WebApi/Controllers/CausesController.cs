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
    public class CausesController : ApiControllerBase
    {
        private GenericRepository<Cause> _causesRepository;
        private GenericRepository<User> _usersRepository;

        public CausesController(
            GenericRepository<Cause> causesRepository,
            GenericRepository<User> usersRepository)
        {
            _causesRepository = causesRepository;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Get all Causes registered for the logged user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public Task<ActionResult<IEnumerable<CauseViewModel>>> GetAsync()
            => ExecuteAsync<IEnumerable<CauseViewModel>>(async () =>
            {
                var userId = User.UserId();
                var causes = await Task.Run(() =>
                    _causesRepository
                        .GetQueryable()
                        .Where(c => c.UserId == userId)
                        .AsEnumerable()
                        .Select(c => (CauseViewModel) c));

                if (!causes.Any())
                    return NoContent();

                return Ok(causes);
            });

        /// <summary>
        /// Get a Cause via id
        /// </summary>
        /// <param name="id">id of the cause</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public Task<ActionResult<CauseViewModel>> GetAsync(Guid id)
            => ExecuteAsync<CauseViewModel>(async () =>
            {
                var userId = User.UserId();
                var cause = await _causesRepository.GetByIdAsync(id);

                if (cause == null)
                    return NotFound();

                if (cause.UserId != userId)
                    return Forbid();

                return Ok((CauseViewModel) cause);
            });

        /// <summary>
        /// Create a New Cause
        /// </summary>
        /// <param name="inputModel">Cause information</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public Task<ActionResult<CauseViewModel>> PostAsync([FromBody] CauseInputModel inputModel)
            => ExecuteAsync<CauseViewModel>(async () =>
            {
                var userId = User.UserId();
                var user = await _usersRepository.GetByIdAsync(userId);
                var cause = new Cause(user, inputModel.Description, inputModel.Icon);

                await _causesRepository.AddAsync(cause);

                return Ok((CauseViewModel) cause);
            });

        /// <summary>
        /// Update a Cause
        /// </summary>
        /// <param name="id">Id of the Cause to be updated</param>
        /// <param name="inputModel">Cause information</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public Task<ActionResult<CauseViewModel>> PutAsync(Guid id, [FromBody] CauseInputModel inputModel)
            => ExecuteAsync<CauseViewModel>(async () =>
            {
                var userId = User.UserId();
                var cause = await _causesRepository.GetByIdAsync(id);

                if (cause == null)
                    return NotFound();

                if (cause.UserId != userId)
                    return Forbid();

                cause.SetDescription(inputModel.Description);
                cause.Icon = inputModel.Icon;

                await _causesRepository.UpdateAsync(cause);

                return Ok((CauseViewModel) cause);
            });

        /// <summary>
        /// Delete a Cause
        /// </summary>
        /// <param name="id">Id of the cause</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public Task<ActionResult> DeleteAsync(Guid id)
            => ExecuteAsync(async () =>
            {
                var userId = User.UserId();
                var cause = await _causesRepository.GetByIdAsync(id);

                if (cause == null)
                    return NotFound();

                if (cause.UserId != userId)
                    return Forbid();

                await _causesRepository.DeleteAsync(cause);

                return Ok();
            });
    }
}