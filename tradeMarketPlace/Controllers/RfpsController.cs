using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using tradeMarketPlace.Models;

namespace tradeMarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class RfpsController : ControllerBase
    {
        private readonly TradeMarketPlaceContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public RfpsController(TradeMarketPlaceContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        // GET: api/Rfps
        [HttpGet]
        [Authorize(Roles = "buyer, admin, seller")]
        public async Task<ActionResult<IEnumerable<Rfp>>> GetRfp()
        {
            return await _context.Rfp.ToListAsync();
        }

        // GET: api/Rfps/5
        [HttpGet("{buyerId}")]
        [Authorize(Roles = "buyer")]
        public async Task<ActionResult<List<Rfp>>> GetRfps(int buyerId)
        {

            if (buyerId == null)
            {
                return Unauthorized();
            }

            var rfps = await _context.Rfp.Where(r => r.CreatedBy == buyerId).ToListAsync();

            if (rfps == null || rfps.Count == 0)
            {
                return NotFound();
            }

            return rfps;
        }

        // GET: api/Rfp/5
        [HttpGet("rfp/{id}")]
        [Authorize(Roles = "buyer")]
        public async Task<ActionResult<List<Rfp>>> GetRfp(int id)
        {

            if (id == null)
            {
                return Unauthorized();
            }

            var rfp = await _context.Rfp.Where(r => r.RfpId == id).ToListAsync();

            if (rfp == null || rfp.Count == 0)
            {
                return NotFound();
            }

            return rfp;
        }

        [HttpGet("{rfpId}/{UserId}")]
        [Authorize(Roles = "seller")]
        public IActionResult GetExistingRfp(int rfpId, int UserId)
        {
            // Retrieve the RFP object from the database
            var rfp = _context.Rfp.FirstOrDefault(r => r.RfpId == rfpId);
            var user = _context.Users.FirstOrDefault(u => u.UserId == UserId);

            if (user.Type != "seller")
            {
                return BadRequest();
            }

            if (rfp == null)
            {
                return NotFound();
            }

            // Retrieve the existing bid, if any
            var existingBid = _context.Bid.FirstOrDefault(b => b.RfpId == rfpId && b.UserId == UserId);

            // Construct the response object
            var response = new ExistingRfpBid
            {
                Rfp = rfp,
                Bid = existingBid
            };

            return Ok(response);
        }



        // PUT: api/Rfps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "buyer")]
        public async Task<IActionResult> PutRfp(int id, Rfp rfp)
        {
            if (id != rfp.RfpId)
            {
                return BadRequest();
            }

            _context.Entry(rfp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RfpExists(id))
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

        // POST: api/Rfps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "buyer, admin")]
        public async Task<ActionResult<Rfp>> PostRfp(Rfp rfp)
        {
            _context.Rfp.Add(rfp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRfp", new { id = rfp.RfpId }, rfp);
        }

        // DELETE: api/Rfps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRfp(int id)
        {
            var rfp = await _context.Rfp.FindAsync(id);
            if (rfp == null)
            {
                return NotFound();
            }

            _context.Rfp.Remove(rfp);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RfpExists(int id)
        {
            return _context.Rfp.Any(e => e.RfpId == id);
        }
    }
}
