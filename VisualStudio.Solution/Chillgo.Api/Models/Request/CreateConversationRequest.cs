namespace Chillgo.Api.Models.Request
{
    public class CreateConversationRequest
    {
        public Guid FirstAccountId { get; set; }
        public string FirstName { get; set; }  // Có thể rỗng
        public Guid SecondAccountId { get; set; }
        public string SecondName { get; set; }  // Có thể rỗng
        public Guid? AIBotId { get; set; }  // Có thể rỗng, cuộc trò chuyện với AI
    }
}
