using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Diporto.Models;
using Diporto.Database;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HashidsNet;

namespace Diporto.Controllers {
  [Authorize]
  [Route("api/rooms")]
  public class RoomMembershipController : Controller {
    private readonly DatabaseContext context;

    private UserManager<User> userManager;
    public RoomMembershipController(DatabaseContext context, UserManager<User> userManager) {
      this.context = context;
      this.userManager = userManager;
    }

    [HttpPost("{roomId:int}/memberships")]
    public async Task<IActionResult> Create(int roomId) {
      var room = context.Rooms.FirstOrDefault(r => r.Id == roomId);
      if (room == null) {
        return NotFound();
      }

      var user = await userManager.GetUserAsync(User);

      var roomMembership = new RoomMembership {
        User = user,
        Room = room
      };

      context.RoomMemberships.Add(roomMembership);
      context.SaveChanges();
      return new ObjectResult(roomMembership);
    }

    [HttpDelete("{roomId:int}/memberships")]
    public async Task<IActionResult> Delete(int roomId) {
      var user = await userManager.GetUserAsync(User);
      var roomMembership = context.RoomMemberships.Where(rm => rm.RoomId == roomId).FirstOrDefault(rm => rm.User == user);
      if (roomMembership == null) {
        return NotFound();
      }
      context.RoomMemberships.Remove(roomMembership);
      context.SaveChanges();
      return new NoContentResult();
    }
  }
}
