using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System.Net;
using System.Net.Mail;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using tradeMarketPlace.Models;

namespace tradeMarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly TradeMarketPlaceContext _context;

        public PurchaseOrdersController(TradeMarketPlaceContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrder()
        {
            return await _context.PurchaseOrder.ToListAsync();
        }

        // GET: api/PurchaseOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrder.FindAsync(id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return purchaseOrder;
        }

        // PUT: api/PurchaseOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseOrder(int id, PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.PurchaseOrderId)
            {
                return BadRequest();
            }

            _context.Entry(purchaseOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderExists(id))
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

        // POST: api/PurchaseOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> PostPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrder.Add(purchaseOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseOrder", new { id = purchaseOrder.PurchaseOrderId }, purchaseOrder);


        }

        // DELETE: api/PurchaseOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrder.FindAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            _context.PurchaseOrder.Remove(purchaseOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("generatepdf")]
        public async Task<IActionResult> GeneratePDF(string InvoiceNo)
        {
            var document = new PdfDocument();
            string htmlcontent = "<h1>Purchase Order</h1>";
            //string htmlcontent = "<div style='display:flex; flex-direction:column; justify-content:center; '>";
            //htmlcontent += "<h2>" + "sddsdad" + "</h2>";
            //htmlcontent += "<h2>Welcome to Nihira Techiees</h2>";

            ////table to from 
            //htmlcontent += "<table style ='border: 1px solid black; padding: 10px'>";
            //htmlcontent += "<thead style='font-weight:bold'>";
            //htmlcontent += "<tr>";
            //htmlcontent += "<td style='border:1px solid #000'> Product Code </td>";
            //htmlcontent += "<td style='border:1px solid #000'> Description </td>";
            //htmlcontent += "<td style='border:1px solid #000'>Qty</td>";
            //htmlcontent += "<td style='border:1px solid #000'>Price</td >";
            //htmlcontent += "<td style='border:1px solid #000'>Total</td>";
            //htmlcontent += "</tr>";
            //htmlcontent += "</thead >";

            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);

            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }

            string Filename = "Invoice_" + InvoiceNo + ".pdf";
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "PDFs", Filename);
            using (FileStream fs = new FileStream(filepath, FileMode.Create))
            {
                await fs.WriteAsync(response, 0, response.Length);
            }

            // Send email
            using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("maneetsingh20018@gmail.com", "uxbjeucjlsqndshr");

                var message = new MailMessage();
                message.From = new MailAddress("maneetsingh20018@gmail.com");
                message.To.Add(new MailAddress("dope.godfather@gmail.com"));
                message.Subject = "Your subject";
                message.Body = "Your email message body";
                message.Attachments.Add(new Attachment(filepath));

                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in CreateTestMessage(): {0}",
                                ex.ToString());
                }
            }

            return File(response, "application/pdf", Filename);
        }



        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrder.Any(e => e.PurchaseOrderId == id);
        }


    }
}
