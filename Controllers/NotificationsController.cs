using HQLM_Backend.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HQLM_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _repo;

        public NotificationsController(INotificationRepository repo) =>
            _repo = repo;

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUser(int userId) =>
            Ok(await _repo.GetByUserIdAsync(userId));

        [HttpPut("{id:int}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var notification = await _repo.GetByIdAsync(id);
            if (notification == null)
                return NotFound();

            notification.IsRead = true;

            await _repo.UpdateAsync(notification);
            await _repo.SaveChangesAsync();

            return NoContent();
        }
    }
}
