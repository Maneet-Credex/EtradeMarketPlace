namespace tradeMarketPlace.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int SenderId { get; set; }

    public int ReciverId { get; set; }

    public string MsgContent { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Status { get; set; } = null!;

    public string Attachment { get; set; } = null!;

    public virtual User? Reciver { get; set; } = null!;

    public virtual User? Sender { get; set; } = null!;
}
