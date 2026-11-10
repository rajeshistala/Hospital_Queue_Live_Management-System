using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQLM_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentsController(IAppointmentRepository repo) =>
            _repo = repo;

        [HttpGet]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _repo.GetByIdAsync(id);
            return appointment == null ? NotFound() : Ok(appointment);
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult> GetByPatient(int patientId) =>
            Ok(await _repo.GetByPatientIdAsync(patientId));

        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(Roles = "Admin,Receptionist,Doctor")]
        public async Task<IActionResult> GetByDoctor(int doctorId) =>
            Ok(await _repo.GetByDoctorIdAsync(doctorId));

        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist,Patient")]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            appointment.Status = "Booked";
            appointment.CreatedAt = DateTime.UtcNow;

            await _repo.AddAsync(appointment);
            await _repo.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = appointment.AppointmentId },
                appointment);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Receptionist,Patient")]
        public async Task<IActionResult> Update(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentId)
                return BadRequest("ID mismatch.");

            if (await _repo.GetByIdAsync(id) == null)
                return NotFound();

            await _repo.UpdateAsync(appointment);
            await _repo.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}/cancel")]
        [Authorize(Roles = "Admin,Receptionist,Patient")]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _repo.GetByIdAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.Status = "Cancelled";

            await _repo.UpdateAsync(appointment);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
