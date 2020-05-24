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
    public class MedicationsController : ApiControllerBase
    {
        private GenericRepository<Medication> _medicationsRepository;
        private GenericRepository<User> _usersRepository;

        public MedicationsController(
            GenericRepository<Medication> medicationsRepository,
            GenericRepository<User> usersRepository)
        {
            _medicationsRepository = medicationsRepository;
            _usersRepository = usersRepository;
        }

        /// <summary>
        /// Get all Medications registered for the logged user
        /// </summary>
        /// <param name="queryInputModel">Possible filters</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public Task<ActionResult<IEnumerable<MedicationViewModel>>> GetAsync([FromQuery] MedicationQueryInputModel queryInputModel = null)
            => ExecuteAsync<IEnumerable<MedicationViewModel>>(async () =>
            {
                var userId = User.UserId();
                var medications = await Task.Run(() =>
                {
                    var query = _medicationsRepository
                        .GetQueryable()
                        .Where(c => c.UserId == userId);

                    if (queryInputModel?.IsActive != null)
                    {
                        var isActive = queryInputModel.IsActive.Value;
                        query = query.Where(c => c.IsActive == isActive);
                    }

                    return query
                        .AsEnumerable()
                        .Select(c => (MedicationViewModel) c);
                });

                if (!medications.Any())
                    return NoContent();

                return Ok(medications);
            });

        /// <summary>
        /// Get a Medication via id
        /// </summary>
        /// <param name="id">id of the medication</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public Task<ActionResult<MedicationViewModel>> GetAsync(Guid id)
            => ExecuteAsync<MedicationViewModel>(async () =>
            {
                var userId = User.UserId();
                var medication = await _medicationsRepository.GetByIdAsync(id);

                if (medication == null)
                    return NotFound();

                if (medication.UserId != userId)
                    return Forbid();

                return Ok((MedicationViewModel) medication);
            });

        /// <summary>
        /// Create a New Medication
        /// </summary>
        /// <param name="inputModel">Medication information</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public Task<ActionResult<MedicationViewModel>> PostAsync([FromBody] MedicationInputModel inputModel)
            => ExecuteAsync<MedicationViewModel>(async () =>
            {
                var userId = User.UserId();
                var user = await _usersRepository.GetByIdAsync(userId);

                var medication = new Medication(user, inputModel.Name, inputModel.Description, inputModel.Start, inputModel.Interval, inputModel.TotalDoses);
                await _medicationsRepository.AddAsync(medication);

                return Ok((MedicationViewModel) medication);
            });

        /// <summary>
        /// Finish taking a medication
        /// </summary>
        /// <param name="id">Id of the medication</param>
        /// <returns></returns>
        [HttpPost("{id}/finish")]
        public Task<ActionResult<MedicationViewModel>> FinishAsync(Guid id)
            => ExecuteAsync<MedicationViewModel>(async () =>
            {
                var userId = User.UserId();
                var medication = await _medicationsRepository.GetByIdAsync(id);

                if (medication == null)
                    return NotFound();

                if (medication.Id != userId)
                    return Forbid();

                medication.Finish();
                await _medicationsRepository.UpdateAsync(medication);

                return Ok((MedicationViewModel) medication);
            });

        /// <summary>
        /// Update a Medication
        /// </summary>
        /// <param name="id">Id of the Medication to be updated</param>
        /// <param name="inputModel">Medication information</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        public Task<ActionResult<MedicationViewModel>> PutAsync(Guid id, [FromBody] MedicationUpdateInputModel inputModel)
            => ExecuteAsync<MedicationViewModel>(async () =>
            {
                var userId = User.UserId();
                var medication = await _medicationsRepository.GetByIdAsync(id);

                if (medication == null)
                    return NotFound();

                if (medication.UserId != userId)
                    return Forbid();

                if (inputModel.Name != null)
                    medication.SetName(inputModel.Name);

                if (inputModel.Description != null)
                    medication.SetDescription(inputModel.Description);

                if (inputModel.Start != null)
                    medication.SetStart(inputModel.Start.Value);

                if (inputModel.Interval != null)
                    medication.SetInterval(inputModel.Interval.Value);

                if (inputModel.TotalDoses != null)
                    medication.SetTotalDoses(inputModel.TotalDoses.Value);

                await _medicationsRepository.UpdateAsync(medication);

                return Ok((MedicationViewModel) medication);
            });

        /// <summary>
        /// Delete a Medication
        /// </summary>
        /// <param name="id">Id of the medication</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public Task<ActionResult> DeleteAsync(Guid id)
            => ExecuteAsync(async () =>
            {
                var userId = User.UserId();
                var medication = await _medicationsRepository.GetByIdAsync(id);

                if (medication == null)
                    return NotFound();

                if (medication.UserId != userId)
                    return Forbid();

                await _medicationsRepository.DeleteAsync(medication);

                return Ok();
            });
    }
}