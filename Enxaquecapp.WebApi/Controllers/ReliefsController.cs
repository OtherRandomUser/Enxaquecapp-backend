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
    public class ReliefsController : ApiControllerBase
    {
        private GenericRepository<Relief> _reliefsRepository;
        private GenericRepository<User> _usersRepository;

        public ReliefsController(
            GenericRepository<Relief> reliefsRepository,
            GenericRepository<User> usersRepository)
        {
            _reliefsRepository = reliefsRepository;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Get all Reliefs registered for the logged user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public Task<ActionResult<IEnumerable<ReliefViewModel>>> GetAsync()
            => ExecuteAsync<IEnumerable<ReliefViewModel>>(async () =>
            {
                var userId = User.UserId();
                var reliefs = await Task.Run(() =>
                    _reliefsRepository
                        .GetQueryable()
                        .Where(c => c.UserId == userId)
                        .AsEnumerable()
                        .Select(c => (ReliefViewModel) c));

                if (!reliefs.Any())
                    return NoContent();

                return Ok(reliefs);
            });

        /// <summary>
        /// Get a Relief via id
        /// </summary>
        /// <param name="id">id of the relief</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public Task<ActionResult<ReliefViewModel>> GetAsync(Guid id)
            => ExecuteAsync<ReliefViewModel>(async () =>
            {
                var userId = User.UserId();
                var relief = await _reliefsRepository.GetByIdAsync(id);

                if (relief == null)
                    return NotFound();

                if (relief.UserId != userId)
                    return Forbid();

                return Ok((ReliefViewModel) relief);
            });

        /// <summary>
        /// Create a New Relief
        /// </summary>
        /// <param name="inputModel">Relief information</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public Task<ActionResult<ReliefViewModel>> PostAsync([FromBody] ReliefInputModel inputModel)
            => ExecuteAsync<ReliefViewModel>(async () =>
            {
                var userId = User.UserId();
                var user = await _usersRepository.GetByIdAsync(userId);
                var relief = new Relief(user, inputModel.Description, inputModel.Icon);

                await _reliefsRepository.AddAsync(relief);

                return Ok((ReliefViewModel) relief);
            });

        /// <summary>
        /// Update a Relief
        /// </summary>
        /// <param name="id">Id of the Relief to be updated</param>
        /// <param name="inputModel">Relief information</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public Task<ActionResult<ReliefViewModel>> PutAsync(Guid id, [FromBody] ReliefInputModel inputModel)
            => ExecuteAsync<ReliefViewModel>(async () =>
            {
                var userId = User.UserId();
                var releif = await _reliefsRepository.GetByIdAsync(id);

                if (releif == null)
                    return NotFound();

                if (releif.UserId != userId)
                    return Forbid();

                releif.SetDescription(inputModel.Description);
                releif.Icon = inputModel.Icon;

                await _reliefsRepository.UpdateAsync(releif);

                return Ok((ReliefViewModel) releif);
            });

        /// <summary>
        /// Delete a Relief
        /// </summary>
        /// <param name="id">Id of the relief</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public Task<ActionResult> DeleteAsync(Guid id)
            => ExecuteAsync(async () =>
            {
                var userId = User.UserId();
                var relief = await _reliefsRepository.GetByIdAsync(id);

                if (relief == null)
                    return NotFound();

                if (relief.UserId != userId)
                    return Forbid();

                await _reliefsRepository.DeleteAsync(relief);

                return Ok();
            });
    }
}