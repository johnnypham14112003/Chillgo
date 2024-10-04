namespace Chillgo.Api.Models.Request
{
    public class CreateMessageRequest
    {
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
    }

}
