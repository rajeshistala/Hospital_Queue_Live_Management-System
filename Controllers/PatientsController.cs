using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQLM_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _repo;

        public PatientsController(IPatientRepository repo) => _repo = repo;

        [HttpGet]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _repo.GetByIdAsync(id);
            return patient == null ? NotFound() : Ok(patient);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Create(Patient patient)
        {
            await _repo.AddAsync(patient);
            await _repo.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = patient.PatientId },
                patient);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> Update(int id, Patient patient)
        {
            if (id != patient.PatientId)
                return BadRequest("ID mismatch.");

            if (await _repo.GetByIdAsync(id) == null)
                return NotFound();

            await _repo.UpdateAsync(patient);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _repo.GetByIdAsync(id);
            if (patient == null)
                return NotFound();

            await _repo.DeleteAsync(patient);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
