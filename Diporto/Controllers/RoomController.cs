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
  public class RoomController : Controller {
    const string hashIdSalt = "95AE4B22-AE51-4F55-BD74-A17CC6990B5F";
    private Hashids hashids = new Hashids(hashIdSalt, 5);

    private readonly DatabaseContext context;
    private UserManager<User> userManager;
    public RoomController(DatabaseContext context, UserManager<User> userManager) {
      this.context = context;
      this.userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Room room) {
      if (!ModelState.IsValid) {
        return BadRequest();
      }

      var user = await userManager.GetUserAsync(User);
      room.Owner = user;
      context.Rooms.Add(room);
      context.SaveChanges();

      room.ShortCode = hashids.Encode(room.Id);
      context.Rooms.Update(room);

      var roomMembership = new RoomMembership {
        Room = room,
        User = user
      };
      context.RoomMemberships.Add(roomMembership);
      context.SaveChanges();
      return new ObjectResult(room);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id) {
      var room = context.Rooms
        .Include(r => r.Owner)
        .Include(r => r.RoomMemberships)
          .ThenInclude(rm => rm.User)
        .FirstOrDefault(r => r.Id == id);
      if (room == null) {
        return NotFound();
      }
      room.Members = room.RoomMemberships.Select(rm => rm.User);
      return new ObjectResult(room);
    }

    [HttpGet("{shortCode}")]
    public IActionResult GetByShortCode(string shortCode) {
      var room = context.Rooms
        .Include(r => r.Owner)
        .Include(r => r.RoomMemberships)
          .ThenInclude(rm => rm.User)
        .FirstOrDefault(r => r.ShortCode == shortCode);
      if (room == null) {
        return NotFound();
      }
      room.Members = room.RoomMemberships.Select(rm => rm.User);
      return new ObjectResult(room);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Room payload) {
      var room = context.Rooms.FirstOrDefault(r => r.Id  == id);
      var user = await userManager.GetUserAsync(User);
      if (!room.Owner.IsAdmin && room.Owner.Id != user.Id) {
        return Forbid();
      }
      room.Name = payload.Name;
      context.Rooms.Update(room);
      context.SaveChanges();
      return new NoContentResult();
    } 

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id) {
      var room = context.Rooms.FirstOrDefault(r => r.Id  == id);
      if (room == null) {
        return NotFound();
      }
      context.Rooms.Remove(room);
      context.SaveChanges();
      return new NoContentResult();
    }
  }
}
