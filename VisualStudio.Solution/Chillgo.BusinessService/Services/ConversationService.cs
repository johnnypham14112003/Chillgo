using AutoMapper;
using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.SharedDTOs;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConversationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ConversationDto>> GetAllConversationsAsync()
        {
            var conversations = await _unitOfWork.ConversationRepository.GetListAsync(c => true);
            return _mapper.Map<List<ConversationDto>>(conversations);
        }

        public async Task<Conversation> CreateConversation(Guid firstAccountId, Guid secondAccountId, string firstName, string secondName, Guid? aiBotId = null)
        {
            var conversation = new Conversation
            {
                FirstAccountId = firstAccountId,
                SecondAccountId = secondAccountId,
                LastUpdated = DateTime.Now,
                Status = "Đã Chuyển Trả"
            };

            // Chỉ thêm FirstName nếu có dữ liệu
            if (!string.IsNullOrEmpty(firstName))
            {
                conversation.FirstName = firstName;
            }

            // Chỉ thêm SecondName nếu có dữ liệu
            if (!string.IsNullOrEmpty(secondName))
            {
                conversation.SecondName = secondName;
            }

            // Nếu AIBotId không rỗng, thiết lập cuộc trò chuyện với AI, nếu không thì là cuộc trò chuyện giữa người với người
            if (aiBotId.HasValue)
            {
                conversation.AibotId = aiBotId;
                conversation.IsHuman = false;  // Cuộc trò chuyện với AI
            }
            else
            {
                conversation.IsHuman = true;  // Cuộc trò chuyện giữa người với người
            }

            await _unitOfWork.ConversationRepository.AddAsync(conversation);
            await _unitOfWork.SaveChangesAsync();

            return conversation;
        }
    }
}
