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
using Microsoft.EntityFrameworkCore;

namespace Enxaquecapp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodesController : ApiControllerBase
    {
        private GenericRepository<Episode> _episodesRepository;
        private GenericRepository<User> _usersRepository;

        private GenericRepository<Cause> _causesRepository;
        private GenericRepository<Local> _localsRepository;
        private GenericRepository<Relief> _reliefsRepository;

        public EpisodesController(
            GenericRepository<Episode> episodesRepository,
            GenericRepository<User> usersRepository,
            GenericRepository<Cause> causesRepository,
            GenericRepository<Local> localsRepository,
            GenericRepository<Relief> reliefsRepository)
        {
            _episodesRepository = episodesRepository;
            _usersRepository = usersRepository;

            _causesRepository = causesRepository;
            _localsRepository = localsRepository;
            _reliefsRepository = reliefsRepository;
        }

        /// <summary>
        /// Get all Episodes registered for the logged user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public Task<ActionResult<IEnumerable<EpisodeViewModel>>> GetAsync()
            => ExecuteAsync<IEnumerable<EpisodeViewModel>>(async () =>
            {
                var userId = User.UserId();
                var episodes = await Task.Run(() =>
                    _episodesRepository
                        .GetQueryable()
                        .Where(c => c.UserId == userId)
                        .Include(c => c.Local)
                        .Include(c => c.Cause)
                        .Include(c => c.Relief)
                        .AsEnumerable()
                        .Select(c => (EpisodeViewModel) c));

                if (!episodes.Any())
                    return NoContent();

                return Ok(episodes);
            });

        /// <summary>
        /// Get a Episode via id
        /// </summary>
        /// <param name="id">id of the episode</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public Task<ActionResult<EpisodeViewModel>> GetAsync(Guid id)
            => ExecuteAsync<EpisodeViewModel>(async () =>
            {
                var userId = User.UserId();
                var episode = await _episodesRepository.GetByIdAsync(id, c => c
                    .Include(r => r.Local)
                    .Include(r => r.Cause)
                    .Include(r => r.Relief)
                );

                if (episode == null)
                    return NotFound();

                if (episode.UserId != userId)
                    return Forbid();

                return Ok((EpisodeViewModel) episode);
            });

        /// <summary>
        /// Create a New Episode
        /// </summary>
        /// <param name="inputModel">Episode information</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public Task<ActionResult<EpisodeViewModel>> PostAsync([FromBody] EpisodeInputModel inputModel)
            => ExecuteAsync<EpisodeViewModel>(async () =>
            {
                var userId = User.UserId();
                var user = await _usersRepository.GetByIdAsync(userId);

                var cause = default(Cause);
                var local = default(Local);
                var relief = default(Relief);

                if (inputModel.CauseId != null)
                {
                    cause = await _causesRepository.GetByIdAsync(inputModel.CauseId.Value);

                    if (cause == null || cause.UserId != userId)
                        return BadRequest("Causa inválida");
                }

                if (inputModel.LocalId != null)
                {
                    local = await _localsRepository.GetByIdAsync(inputModel.LocalId.Value);

                    if (local == null || local.UserId != userId)
                        return BadRequest("Local inválido");
                }

                if (inputModel.ReliefId != null)
                {
                    relief = await _reliefsRepository.GetByIdAsync(inputModel.ReliefId.Value);

                    if (relief == null || relief.UserId != userId)
                        return BadRequest("Alívio inválido");
                }

                var episode = new Episode(
                    user,
                    inputModel.Start,
                    inputModel.End,
                    inputModel.Intensity,
                    inputModel.ReleafWorked,
                    local,
                    cause,
                    relief
                );

                await _episodesRepository.AddAsync(episode);

                return Ok((EpisodeViewModel) episode);
            });

        /// <summary>
        /// Update an Episode
        /// </summary>
        /// <param name="id">Id of the Episode to be updated</param>
        /// <param name="inputModel">Episode information</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        public Task<ActionResult<EpisodeViewModel>> PutAsync(Guid id, [FromBody] EpisodeUpdateInputModel inputModel)
            => ExecuteAsync<EpisodeViewModel>(async () =>
            {
                var userId = User.UserId();
                var episode = await _episodesRepository.GetByIdAsync(id);

                if (episode == null)
                    return NotFound();

                if (episode.UserId != userId)
                    return Forbid();

                if (inputModel.Start != null)
                    episode.SetStart(inputModel.Start.Value);

                if (inputModel.End != null)
                    episode.SetEnd(inputModel.End);

                if (inputModel.ClearEnd)
                    episode.SetEnd(null);

                if (inputModel.Intensity != null)
                    episode.SetIntensity(inputModel.Intensity.Value);

                if (inputModel.ReleafWorked != null)
                    episode.ReleafWorked = inputModel.ReleafWorked.Value;

                if (inputModel.CauseId != null)
                {
                    var cause = await _causesRepository.GetByIdAsync(inputModel.CauseId.Value);

                    if (cause == null || cause.UserId != userId)
                        return BadRequest("Causa inválida");

                    episode.SetCause(cause);
                }

                if (inputModel.ClearCause)
                    episode.SetCause(null);

                if (inputModel.LocalId != null)
                {
                    var local = await _localsRepository.GetByIdAsync(inputModel.LocalId.Value);

                    if (local == null || local.UserId != userId)
                        return BadRequest("Local inválido");

                    episode.SetLocal(local);
                }

                if (inputModel.ClearLocal)
                    episode.SetLocal(null);

                if (inputModel.ReliefId != null)
                {
                    var relief = await _reliefsRepository.GetByIdAsync(inputModel.ReliefId.Value);

                    if (relief == null || relief.UserId != userId)
                        return BadRequest("Alívio inválido");

                    episode.SetRelief(relief);
                }

                if (inputModel.ClearRelief)
                    episode.SetRelief(null);

                await _episodesRepository.UpdateAsync(episode);

                return Ok((EpisodeViewModel) episode);
            });

        /// <summary>
        /// Delete an Episode
        /// </summary>
        /// <param name="id">Id of the episode</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public Task<ActionResult> DeleteAsync(Guid id)
            => ExecuteAsync(async () =>
            {
                var userId = User.UserId();
                var episode = await _episodesRepository.GetByIdAsync(id);

                if (episode == null)
                    return NotFound();

                if (episode.UserId != userId)
                    return Forbid();

                await _episodesRepository.DeleteAsync(episode);

                return Ok();
            });
    }
}