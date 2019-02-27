using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using assignment.ModelViews;
using Microsoft.AspNetCore.Authorization;

namespace assignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BoatsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BoatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Boats
        [HttpGet]
        [Authorize(Roles = "Admin,Member")]
        public async Task<ActionResult<IEnumerable<Boat>>> GetBoats()
        {
            return await _context.Boats.ToListAsync();
        }

        [HttpGet("test")]
        public async Task<ActionResult<IEnumerable<Boat>>> GetBoatsTest()
        {
            return await _context.Boats.ToListAsync();
        }

        // GET: api/Boats/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Member")]
        public async Task<ActionResult<Boat>> GetBoat(int id)
        {
            var boat = await _context.Boats.FindAsync(id);

            if (boat == null)
            {
                return NotFound();
            }

            return boat;
        }

        // PUT: api/Boats/5
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> PutBoat(int id, Boat boat)
        {
            if (id != boat.BoatId)
            {
                return BadRequest();
            }

            _context.Entry(boat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoatExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Boats
        [HttpPost]
        [Authorize(Roles = "Admin,Member")]
        public async Task<ActionResult<Boat>> PostBoat(Boat boat)
        {
            _context.Boats.Add(boat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoat", new { id = boat.BoatId }, boat);
        }

        // DELETE: api/Boats/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<Boat>> DeleteBoat(int id)
        {
            var boat = await _context.Boats.FindAsync(id);
            if (boat == null)
            {
                return NotFound();
            }

            _context.Boats.Remove(boat);
            await _context.SaveChangesAsync();

            return boat;
        }

        private bool BoatExists(int id)
        {
            return _context.Boats.Any(e => e.BoatId == id);
        }
    }
}
