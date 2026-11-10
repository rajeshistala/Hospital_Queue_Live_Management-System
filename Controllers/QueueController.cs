using HQLM_Backend.Models;
using HQLM_Backend.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQLM_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QueueController : ControllerBase
    {
        private readonly IQueueRepository _repo;

        public QueueController(IQueueRepository repo) => _repo = repo;

        [HttpGet("daily")]
        [Authorize(Roles = "Admin,Receptionist,Doctor")]
        public async Task<IActionResult> GetDailyQueue(
            [FromQuery] DateTime? date)
        {
            var queueDate = date ?? DateTime.Today;
            return Ok(await _repo.GetDailyQueueAsync(queueDate));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var token = await _repo.GetByIdAsync(id);
            return token == null ? NotFound() : Ok(token);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> GenerateToken(QueueToken token)
        {
            token.Status = "Waiting";
            token.QueueDate = DateTime.Today;

            await _repo.AddAsync(token);
            await _repo.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = token.QueueTokenId },
                token);
        }

        [HttpPut("{id:int}/call")]
        [Authorize(Roles = "Admin,Receptionist,Doctor")]
        public async Task<IActionResult> CallToken(int id)
        {
            var token = await _repo.GetByIdAsync(id);
            if (token == null)
                return NotFound();

            token.Status = "Called";
            token.CalledAt = DateTime.UtcNow;

            await _repo.UpdateAsync(token);
            await _repo.SaveChangesAsync();

            return Ok(token);
        }

        [HttpPut("{id:int}/complete")]
        [Authorize(Roles = "Admin,Receptionist,Doctor")]
        public async Task<IActionResult> CompleteToken(int id)
        {
            var token = await _repo.GetByIdAsync(id);
            if (token == null)
                return NotFound();

            token.Status = "Completed";
            token.CompletedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(token);
            await _repo.SaveChangesAsync();

            return Ok(token);
        }

        [HttpPut("{id:int}/skip")]
        [Authorize(Roles = "Admin,Receptionist,Doctor")]
        public async Task<IActionResult> SkipToken(int id)
        {
            var token = await _repo.GetByIdAsync(id);
            if (token == null)
                return NotFound();

            token.Status = "Skipped";

            await _repo.UpdateAsync(token);
            await _repo.SaveChangesAsync();

            return Ok(token);
        }

        [HttpPut("{id:int}/recall")]
        [Authorize(Roles = "Admin,Receptionist,Doctor")]
        public async Task<IActionResult> RecallToken(int id)
        {
            var token = await _repo.GetByIdAsync(id);
            if (token == null)
                return NotFound();

            token.Status = "Called";
            token.CalledAt = DateTime.UtcNow;

            await _repo.UpdateAsync(token);
            await _repo.SaveChangesAsync();

            return Ok(token);
        }
    }
}
