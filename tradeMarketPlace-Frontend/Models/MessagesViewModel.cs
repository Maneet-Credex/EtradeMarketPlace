namespace tradeMarketPlace_Frontend.Models
{
    public class MessagesViewModel
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
    public class MessageJsonModel
    {
        public int MessageId { get; set; }

        public int SenderId { get; set; }

        public int ReciverId { get; set; }

        public string MsgContent { get; set; } = null!;

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "valid";

        public string Attachment { get; set; } = "Dummy";
    }
}
