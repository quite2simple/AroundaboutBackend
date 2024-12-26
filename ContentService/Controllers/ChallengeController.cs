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
    public class ChallengeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChallengeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Challenge
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Challenge>>> GetChallenges()
        {
            return await _context.Challenges.ToListAsync();
        }

        // GET: api/Challenge/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Challenge>> GetChallenge(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);

            if (challenge == null)
            {
                return NotFound();
            }

            return challenge;
        }

        // PUT: api/Challenge/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "SuperAdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChallenge(int id, Challenge challenge)
        {
            if (id != challenge.Id)
            {
                return BadRequest();
            }

            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeExists(id))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // POST: api/Challenge
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "ExpertPolicy")]
        [HttpPost]
        public async Task<ActionResult<Challenge>> PostChallenge([FromBody] CreateChallengeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var challenge = new Challenge
            {
                Name = dto.Name,
                Description = dto.Description,
                ModelUrl = dto.ModelURL,
                OwnerEmail = dto.OwnerEmail
            };

            if (!ChallengeOwnedByCurrentUser(challenge))
            {
                return Forbid();
            }
            
            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChallenge", new { id = challenge.Id }, challenge);
        }

        // DELETE: api/Challenge/5
        [Authorize(Policy = "ExpertPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallenge(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }
            
            if (!ChallengeOwnedByCurrentUser(challenge))
            {
                return Forbid();
            }

            _context.Challenges.Remove(challenge);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChallengeExists(int id)
        {
            return _context.Challenges.Any(e => e.Id == id);
        }

        private bool ChallengeOwned(Challenge challenge, string email)
        {
            return challenge.OwnerEmail == email;
        }
        
        private bool ChallengeOwnedByCurrentUser(Challenge challenge)
        {
            return challenge.OwnerEmail == User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
