using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tradeMarketPlace.Models;

namespace tradeMarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class BidsController : ControllerBase
    {
        private readonly TradeMarketPlaceContext _context;

        public BidsController(TradeMarketPlaceContext context)
        {
            _context = context;
        }

        // GET: api/Bids
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBid()
        {
            return await _context.Bid.ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Bid>> GetBid(int id)
        {
            var bid = await _context.Bid
             .Include(b => b.Rfp)
             .ThenInclude(r => r.User)
             .Include(b => b.Rfp)
             .ThenInclude(r => r.Product)
             .ThenInclude(p => p.ProductCategory) // Include ProductCategory
             .ThenInclude(p => p.SubCategories) // Include ProductSubCategory
             .Include(b => b.User)
             .FirstOrDefaultAsync(b => b.BidId == id);


            if (bid == null)
            {
                return NotFound();
            }

            if (bid.Rfp == null || bid.User == null || bid.Rfp.User == null || bid.Rfp.Product == null || bid.Rfp.ProductCategory == null || bid.Rfp.ProductSubCategory == null)
            {
                return BadRequest();
            }


            // Select only the desired RFP properties
            var rfpData = new RfpPdf
            {
                Title = bid.Rfp.RfpName,
                RfpID = bid.Rfp.RfpId,
                RfpQuantity = bid.Rfp.Quantity,
                RfpPrice = bid.Rfp.MaxPrice,
                RfpLastDate = bid.Rfp.LastDate,
                Description = bid.Rfp.RfpDescription,
                ProductCategory = bid.Rfp.ProductCategory.Name,
                ProductSubCategory = bid.Rfp.ProductSubCategory.Name,
                ProductName = bid.Rfp.Product.Name,
                ProductDescription = bid.Rfp.Product.Description,

                Buyer = new User
                {
                    FirstName = bid.Rfp.User.FirstName,
                    LastName = bid.Rfp.User.LastName,
                    Email = bid.Rfp.User.Email,
                    Type = bid.Rfp.User.Type,
                    OrganisationName = bid.Rfp.User.OrganisationName,
                    ContactNumber = bid.Rfp.User.ContactNumber,
                }
            };

            // Select only the desired User properties for the seller
            var sellerData = new User
            {
                FirstName = bid.User.FirstName,
                LastName = bid.Rfp.User.LastName,
                Email = bid.User.Email,
                Type = bid.User.Type,
                OrganisationName = bid.User.OrganisationName,
                ContactNumber = bid.User.ContactNumber,
            };

            // Return the bid along with the selected RFP, User, and Seller data
            return new ObjectResult(new
            {
                Id = bid.BidId,
                BidPrice = bid.Price,
                RFP = rfpData,
                Seller = sellerData
            });
        }





        // GET: api/Bids/5
        [HttpGet("BidsForRfp/{rfpId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<object>>> GetBidsForRfp(int rfpId)
        {
            var bids = await _context.Bid
                .Where(b => b.RfpId == rfpId)
                .Include(b => b.User)
                .Select(b => new
                {
                    BidId = b.BidId,
                    Price = b.Price,
                    Date = b.BidDateTime,
                    Comments = b.Comments,
                    OrganisationName = b.User.OrganisationName,
                    UserId = b.User.UserId,
                    UserName = b.User.FirstName,
                    RfpName = b.Rfp.RfpName,
                })
                .ToListAsync();

            if (!bids.Any())
            {
                return NotFound();
            }

            return bids;
        }


        // PUT: api/Bids/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutBid(int id, Bid bid)
        {
            if (id != bid.BidId)
            {
                return BadRequest();
            }

            _context.Entry(bid).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BidExists(id))
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

        // POST: api/Bids
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "seller")]
        public async Task<ActionResult<Bid>> PostBid(Bid bid)
        {
            _context.Bid.Add(bid);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBid", new { id = bid.BidId }, bid);
        }

        // DELETE: api/Bids/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            var bid = await _context.Bid.FindAsync(id);
            if (bid == null)
            {
                return NotFound();
            }

            _context.Bid.Remove(bid);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BidExists(int id)
        {
            return _context.Bid.Any(e => e.BidId == id);
        }
    }
}
