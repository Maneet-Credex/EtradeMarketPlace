using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tradeMarketPlace.Models;

namespace tradeMarketPlace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly TradeMarketPlaceContext _context;

        public MessagesController(TradeMarketPlaceContext context)
        {
            _context = context;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessage()
        {
            return await _context.Message.ToListAsync();
        }

        // GET: api/Messages/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Message>> GetMessage(int id)
        //{
        //    var message = await _context.Message.FindAsync(id);

        //    if (message == null)
        //    {
        //        return NotFound();
        //    }

        //    return message;
        //}


        [HttpGet("{senderId}/{receiverId}")]
        public async Task<ActionResult<List<MessageWithUser>>> GetMessages(int senderId, int receiverId)
        {
            // Find all messages where the sender_id matches the specified sender ID and the receiver_id matches the specified receiver ID
            var messages = await _context.Message
                .Where(m => (m.SenderId == senderId && m.ReciverId == receiverId) || (m.SenderId == receiverId && m.ReciverId == senderId))
                .ToListAsync();

            if (messages == null || messages.Count == 0)
            {
                return NotFound();
            }

            // Get the receiver data from the user table
            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.UserId == receiverId);

            // Map the messages and receiver data to a new class that contains both
            var messagesWithUser = messages.Select(m => new MessageWithUser
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                ReceiverId = m.ReciverId,
                MessageContent = m.MsgContent,
                Date = m.Date,
                Status = m.Status,
                Attachment = m.Attachment,
                ReceiverName = receiver.FirstName,
                ReceiverEmail = receiver.Email,
                ReceiverPhone = receiver.ContactNumber
            }).ToList();

            return messagesWithUser;
        }


        [HttpGet("{senderId}")]
        public async Task<ActionResult<List<MessageWithUser>>> GetMessages(int senderId)
        {
            // Get the last message from each receiver for the specified sender
            var lastMessages = await _context.Message
                .Where(m => m.SenderId == senderId)
                .GroupBy(m => m.ReciverId)
                .Select(g => g.OrderByDescending(m => m.Date).FirstOrDefault())
                .ToListAsync();

            // If no messages are found, return an empty array
            if (lastMessages == null || lastMessages.Count == 0)
            {
                return new List<MessageWithUser>();
            }

            // Map the last messages and receiver data to a new class that contains both
            var messagesWithUser = new List<MessageWithUser>();

            foreach (var m in lastMessages)
            {
                // Get the receiver data from the user table
                var receiver = await _context.Users.FirstOrDefaultAsync(u => u.UserId == m.ReciverId);

                messagesWithUser.Add(new MessageWithUser
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReciverId,
                    MessageContent = m.MsgContent,
                    Date = m.Date,
                    Status = m.Status,
                    Attachment = m.Attachment,
                    ReceiverName = receiver.FirstName,
                    ReceiverEmail = receiver.Email,
                    ReceiverPhone = receiver.ContactNumber
                });
            }

            return messagesWithUser;
        }







        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, Message message)
        {
            if (id != message.MessageId)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.MessageId }, message);
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.MessageId == id);
        }
    }
}
