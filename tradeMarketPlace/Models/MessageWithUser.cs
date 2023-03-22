namespace tradeMarketPlace.Models
{
    public class MessageWithUser
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageContent { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Attachment { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverPhone { get; set; }
    }

}
