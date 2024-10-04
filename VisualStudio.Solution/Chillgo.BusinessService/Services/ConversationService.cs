using AutoMapper;
using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.SharedDTOs;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ConversationDto> GetConversationByIdAsync(Guid conversationId)
        {
            var conversation = await _unitOfWork.ConversationRepository.GetByIdAsync(conversationId);
            if (conversation == null)
            {
                throw new KeyNotFoundException("Conversation not found");
            }
            return _mapper.Map<ConversationDto>(conversation);
        }

        public async Task<List<ConversationDto>> GetConversationsByAccountIdAsync(Guid accountId)
        {
            var conversations = await _unitOfWork.ConversationRepository
                .GetListAsync(c => c.FirstAccountId == accountId || c.SecondAccountId == accountId);

            return _mapper.Map<List<ConversationDto>>(conversations);
        }

        public async Task<PaginatedMessagesDto> GetMessagesByConversationIdAsync(Guid conversationId, int page, int pageSize, string orderBy, string orderDirection)
        {
            var query = _unitOfWork.MessageRepository
                .Entities
                .Where(m => m.ConversationId == conversationId);

            // Apply ordering
            query = orderBy.ToLower() switch
            {
                "senttime" => orderDirection.ToLower() == "asc"
                              ? query.OrderBy(m => m.SentTime)
                              : query.OrderByDescending(m => m.SentTime),
                _ => query.OrderByDescending(m => m.SentTime) // Default ordering by SentTime descending
            };

            // Pagination logic
            var totalMessages = await query.CountAsync();
            var messages = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Convert entities to DTOs
            var messageDtos = _mapper.Map<List<MessageDto>>(messages);

            return new PaginatedMessagesDto
            {
                ConversationId = conversationId,
                Messages = messageDtos,
                TotalPages = (int)Math.Ceiling(totalMessages / (double)pageSize),
                CurrentPage = page
            };
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

        public async Task DeleteConversationByIdAsync(Guid conversationId)
        {
            var conversation = await _unitOfWork.ConversationRepository.GetByIdAsync(conversationId);
            if (conversation == null)
            {
                throw new KeyNotFoundException("Conversation không tìm thấy");
            }

            // Retrieve and delete all related messages
            var messages = await _unitOfWork.MessageRepository.GetListAsync(m => m.ConversationId == conversationId);
            if (messages.Any())
            {
                await _unitOfWork.MessageRepository.DeleteRangeAsync(messages);
            }

            // Delete the conversation itself
            await _unitOfWork.ConversationRepository.DeleteAsync(conversation);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
