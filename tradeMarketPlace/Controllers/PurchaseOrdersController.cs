using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
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


        [HttpPost("generatepdf")]

        public async Task<IActionResult> GeneratePDF(PurchaseOrderPdf model)
        {
            var buyer = model.Rfp.Buyer;
            var seller = model.Seller;
            // Create a new PDF document
            var document = new Document();
            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            // Set document properties
            document.Open();
            document.AddAuthor("E-Trade Market Place");
            document.AddCreator("E-Trade Market Place");
            document.AddKeywords($"{model.Rfp.Title} Purchase Order, Invoice");
            document.AddSubject("Purchase Order");
            document.AddTitle("Purchase Order");

            // Add logo

            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "util", "credex-technology-Logo.png");
            var logo = Image.GetInstance(logoPath);
            logo.ScaleToFit(150f, 150f);
            logo.SpacingAfter = 10f;
            logo.Alignment = Element.ALIGN_RIGHT;
            document.Add(logo);

            // Add heading
            var heading = new Paragraph("Purchase Order", new Font(Font.FontFamily.TIMES_ROMAN, 24f, Font.BOLD));
            heading.Alignment = Element.ALIGN_LEFT;
            heading.SpacingAfter = 20f;
            document.Add(heading);

            var fromToTable = new PdfPTable(2);

            // Set the width percentage of the table
            fromToTable.WidthPercentage = 100;

            // Add the header row to the table
            fromToTable.AddCell(new PdfPCell(new Phrase("FROM", new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD))));
            fromToTable.AddCell(new PdfPCell(new Phrase("To", new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD))));

            // Add the data row to the table
            fromToTable.AddCell(new PdfPCell(new Phrase($"{buyer.OrganisationName}\nATTN: {buyer.FirstName}\nEmail: {buyer.Email}\nPhone: +91{buyer.ContactNumber}", new Font(Font.FontFamily.HELVETICA, 12f))));
            fromToTable.AddCell(new PdfPCell(new Phrase($"{seller.OrganisationName}\nATTN: {seller.FirstName}\nEmail: {seller.Email}\nPhone: +91{seller.ContactNumber}", new Font(Font.FontFamily.HELVETICA, 12f))));

            // Add the table to the document

            // Add table
            var table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            // Add column headers
            table.AddCell(new PdfPCell(new Phrase("Item Name", new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("RFP Titile", new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Quantity", new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Price", new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("tax", new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD))));
            table.AddCell(new PdfPCell(new Phrase("Total", new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD))));

            // Add table data
            table.AddCell(new PdfPCell(new Phrase(model.Rfp.ProductName, new Font(Font.FontFamily.HELVETICA, 12f))));
            table.AddCell(new PdfPCell(new Phrase(model.Rfp.Title, new Font(Font.FontFamily.HELVETICA, 12f))));
            table.AddCell(new PdfPCell(new Phrase($"{model.Rfp.RfpQuantity}", new Font(Font.FontFamily.HELVETICA, 12f))));
            table.AddCell(new PdfPCell(new Phrase($"${model.BidPrice}", new Font(Font.FontFamily.HELVETICA, 12f))));
            table.AddCell(new PdfPCell(new Phrase("18%", new Font(Font.FontFamily.HELVETICA, 12f))));
            table.AddCell(new PdfPCell(new Phrase($"${model.BidPrice + ((model.BidPrice) * 18) / 100}", new Font(Font.FontFamily.HELVETICA, 12f))));

            // Add table to document
            document.Add(fromToTable);
            document.Add(table);



            // Close the document
            document.Close();

            // Send the PDF as a response and attach it to an email
            var bytes = output.ToArray();

            // Send email with PDF attachment
            var message = new MailMessage();
            message.From = new MailAddress("maneetsingh20018@gmail.com");
            message.To.Add(new MailAddress($"{seller.Email}"));
            message.Subject = $"Purchase Order For {model.Rfp.ProductName}";
            message.Body = $"Congratulation {model.Seller.FirstName} you have won bid for {model.Rfp.RfpQuantity} {model.Rfp.Title} Please find the attached Purchase Order in attachment";

            // Attach PDF to email
            message.Attachments.Add(new Attachment(new MemoryStream(bytes), "PurchaseOrder.pdf"));

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("maneetsingh20018@gmail.com", "uxbjeucjlsqndshr");

                await smtp.SendMailAsync(message);
            }

            // Return PDF file as response
            return File(bytes, "application/pdf", $"{model.Seller.OrganisationName}PurchaseOrderInvoice.pdf");

        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrder.Any(e => e.PurchaseOrderId == id);
        }


    }
}

// Send email
//using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587))
//{
//    client.EnableSsl = true;
//    client.Timeout = 10000;
//    client.DeliveryMethod = SmtpDeliveryMethod.Network;
//    client.UseDefaultCredentials = false;
//    client.Credentials = new NetworkCredential("maneetsingh20018@gmail.com", "uxbjeucjlsqndshr");

//    var message = new MailMessage();
//    message.From = new MailAddress("maneetsingh20018@gmail.com");
//    message.To.Add(new MailAddress("dope.godfather@gmail.com"));
//    message.Subject = "Bid Get Accepted";
//    message.Body = "Congratulation your bid has been get accecpted";
//    message.Attachments.Add(new Attachment(message));

//    try
//    {
//        client.Send(message);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine("Exception caught in CreateTestMessage(): {0}",
//                    ex.ToString());
//    }
//}

//return File(response, "application/pdf", Filename);