using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SignalRexamples.Data;
using SignalRexamples.Models;

namespace SignalRexamples.Controllers
{
    public class ChatRoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChatRoomsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult ChatRoom()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            chatViewModel cv = new()
            {
                Rooms = _context.ChatRooms.ToList(),
                MaxRoomAllowed = 4,
                UserId = userId
            };
            return View(cv);
        }
        public IActionResult AdvancedChat()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            chatViewModel cv = new()
            {
                Rooms = _context.ChatRooms.ToList(),
                MaxRoomAllowed = 4,
                UserId = userId
            };
            return View(cv);
        }

        [HttpGet]
        [Route("/[controller]/GetChatRoom")]
        public async Task<ActionResult<object>> GetChatRoom()
        {
            return await _context.ChatRooms.ToListAsync();
        }

     

        [HttpPost]
        [Route("/[controller]/PostChatRoom")]
        public async Task<IActionResult> PostChatRoom( [FromBody]ChatRoom chatRoom)
        {
            if (chatRoom.Name == null)
            {
                return Problem("Chat Room Name Required.");
            }
                _context.Add(chatRoom);
                await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetChatRoom", new { id = chatRoom.Id },chatRoom);
        }


        [HttpDelete]
        [Route("/[controller]/DelecteChatRoom/{id}")]
        public async Task<IActionResult> DelecteChatRoom(int id)
        {
            var chatRoom = await _context.ChatRooms.FindAsync(id);
            if (chatRoom == null)
            {
                return Problem("Invalid Chat Room ID.");
            }
                _context.ChatRooms.Remove(chatRoom);
            

            await _context.SaveChangesAsync();
            var room = await _context.ChatRooms.FirstOrDefaultAsync();
            return Ok(new { deleted = id, selected = (room == null ? 0 : room.Id) });
           // return CreatedAtAction("GetChatRoom", new { id = chatRoom.Id }, chatRoom);
        }

        [HttpGet]
        [Route("/[controller]/GetChatUsers")]
        public async Task<ActionResult<object>> GetChatUsers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = await _context.Users.ToListAsync();
            if(userId ==null) 
                return NotFound();
            return users.Where(u => u.Id != userId).Select(u => new { u.Id, u.UserName }).ToList();
        }

    }
}
