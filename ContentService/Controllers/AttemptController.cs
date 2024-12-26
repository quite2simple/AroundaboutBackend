using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContentService.Models;
using ContentService.DTO;
using Microsoft.AspNetCore.Authorization;

namespace ContentService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttemptController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttemptController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Attempt
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attempt>>> GetAttempts()
        {
            return await _context.Attempts.ToListAsync();
        }

        // GET: api/Attempt/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attempt>> GetAttempt(int id)
        {
            var attempt = await _context.Attempts.FindAsync(id);

            if (attempt == null)
            {
                return NotFound();
            }

            return attempt;
        }

        [Authorize(Policy = "ExpertPolicy")]
        [HttpPatch("check")]
        public async Task<IActionResult> CheckAttempt([FromBody] CheckAttemptDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var attempt = await _context.Attempts.FindAsync(dto.AttemptId);

            if (attempt == null)
            {
                return BadRequest();
            }
            
            attempt.Checked = dto.Checked;
            attempt.CheckedAt = DateTime.UtcNow;
            
            if (dto.Successful != null)
            {
                attempt.Successful = dto.Successful;
            }
            _context.Attempts.Update(attempt);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        // POST: api/Attempt
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Attempt>> PostAttempt([FromBody] CreateAttemptDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var challenge = await _context.Challenges.Include(c => c.Attempts).
                FirstOrDefaultAsync(c => c.Id == dto.ChallengeId);

            if (challenge == null)
            {
                return BadRequest();
            }
            
            var attempt = new Attempt
            {
                ChallengeId = dto.ChallengeId,
                OwnerEmail = User.FindFirst(ClaimTypes.Email)?.Value,
                Body = dto.Body,
                Checked = false,
                CreatedAt = DateTime.UtcNow
            };
            challenge.Attempts.Add(attempt);
            await _context.SaveChangesAsync();

            // return CreatedAtAction("GetAttempt", new { id = attempt.Id }, attempt);
            return NoContent();
        }

        // DELETE: api/Attempt/5
        [Authorize(Policy = "SuperAdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttempt(int id)
        {
            var attempt = await _context.Attempts.FindAsync(id);
            if (attempt == null)
            {
                return NotFound();
            }

            _context.Attempts.Remove(attempt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AttemptExists(int id)
        {
            return _context.Attempts.Any(e => e.Id == id);
        }
    }
}
