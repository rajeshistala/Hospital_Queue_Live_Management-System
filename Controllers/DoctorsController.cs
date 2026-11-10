using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQLM_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DoctorsController:ControllerBase
    {
        private readonly IDoctorRepository _repo;

        public DoctorsController(IDoctorRepository repo) => _repo = repo;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _repo.GetByIdAsync(id);
            return doctor == null ? NotFound() : Ok(doctor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            await _repo.AddAsync(doctor);
            await _repo.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = doctor.DoctorId },
                doctor);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId)
                return BadRequest("ID mismatch.");

            if (await _repo.GetByIdAsync(id) == null)
                return NotFound();

            await _repo.UpdateAsync(doctor);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _repo.GetByIdAsync(id);
            if (doctor == null)
                return NotFound();

            await _repo.DeleteAsync(doctor);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
