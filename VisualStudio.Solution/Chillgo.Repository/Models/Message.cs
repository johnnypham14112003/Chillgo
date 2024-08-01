﻿namespace Chillgo.Repository.Models;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime SentTime { get; set; }

    public string Status { get; set; } = null!;

    public Guid? SenderId { get; set; }

    public Guid? BotReplyId { get; set; }

    public virtual BotAi? BotReply { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual Account? Sender { get; set; }
}
