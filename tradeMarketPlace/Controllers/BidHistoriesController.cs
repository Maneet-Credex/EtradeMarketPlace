using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tradeMarketPlace.Models;

namespace tradeMarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidHistoriesController : ControllerBase
    {
        private readonly TradeMarketPlaceContext _context;

        public BidHistoriesController(TradeMarketPlaceContext context)
        {
            _context = context;
        }

        // GET: api/BidHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidHistory>>> GetBidHistory()
        {
            return await _context.BidHistory.ToListAsync();
        }

        // GET: api/BidHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BidHistory>> GetBidHistory(int id)
        {
            var bidHistory = await _context.BidHistory.FindAsync(id);

            if (bidHistory == null)
            {
                return NotFound();
            }

            return bidHistory;
        }

        // PUT: api/BidHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBidHistory(int id, BidHistory bidHistory)
        {
            if (id != bidHistory.Id)
            {
                return BadRequest();
            }

            _context.Entry(bidHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BidHistoryExists(id))
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

        // POST: api/BidHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BidHistory>> PostBidHistory(BidHistory bidHistory)
        {
            _context.BidHistory.Add(bidHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBidHistory", new { id = bidHistory.Id }, bidHistory);
        }

        // DELETE: api/BidHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBidHistory(int id)
        {
            var bidHistory = await _context.BidHistory.FindAsync(id);
            if (bidHistory == null)
            {
                return NotFound();
            }

            _context.BidHistory.Remove(bidHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BidHistoryExists(int id)
        {
            return _context.BidHistory.Any(e => e.Id == id);
        }
    }
}
