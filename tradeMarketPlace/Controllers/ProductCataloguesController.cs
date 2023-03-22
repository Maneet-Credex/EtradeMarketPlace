using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tradeMarketPlace.Models;

namespace tradeMarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCataloguesController : ControllerBase
    {
        private readonly TradeMarketPlaceContext _context;

        public ProductCataloguesController(TradeMarketPlaceContext context)
        {
            _context = context;
        }

        // GET: api/ProductCatalogues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCatalogue>>> GetProductCatalogue()
        {
            if (_context.ProductCatalogue == null)
            {
                return NotFound();
            }
            return await _context.ProductCatalogue.ToListAsync();
        }

        // GET: api/ProductCatalogues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCatalogue>> GetProductCatalogue(int id)
        {
            if (_context.ProductCatalogue == null)
            {
                return NotFound();
            }
            var productCatalogue = await _context.ProductCatalogue.FindAsync(id);

            if (productCatalogue == null)
            {
                return NotFound();
            }

            return productCatalogue;
        }

        [HttpGet("{subCategoryId}/{productCategoryId}")]
        public async Task<ActionResult<IEnumerable<ProductCatalogue>>> GetProductCatalogue(int subCategoryId, int productCategoryId)
        {
            var productCatalogue = await _context.ProductCatalogue.Where(pc => pc.SubCategoryId == subCategoryId && pc.ProductCategoryId == productCategoryId).ToListAsync();

            if (productCatalogue == null)
            {
                return NotFound();
            }

            return productCatalogue;
        }

        // PUT: api/ProductCatalogues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCatalogue(int id, ProductCatalogue productCatalogue)
        {
            if (id != productCatalogue.Id)
            {
                return BadRequest();
            }

            _context.Entry(productCatalogue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCatalogueExists(id))
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

        // POST: api/ProductCatalogues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCatalogue>> PostProductCatalogue(ProductCatalogue productCatalogue)
        {
            if (_context.ProductCatalogue == null)
            {
                return Problem("Entity set 'TradeMarketPlaceContext.ProductCatalogue'  is null.");
            }
            _context.ProductCatalogue.Add(productCatalogue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCatalogue", new { id = productCatalogue.Id }, productCatalogue);
        }

        // DELETE: api/ProductCatalogues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCatalogue(int id)
        {
            if (_context.ProductCatalogue == null)
            {
                return NotFound();
            }
            var productCatalogue = await _context.ProductCatalogue.FindAsync(id);
            if (productCatalogue == null)
            {
                return NotFound();
            }

            _context.ProductCatalogue.Remove(productCatalogue);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductCatalogueExists(int id)
        {
            return (_context.ProductCatalogue?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
