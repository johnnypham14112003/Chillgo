using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Message> CreateMessageAsync(Message message)
        {
            await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();
            return message;
        }

        public async Task<Message> UpdateMessageStatusAsync(Guid messageId, string status)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(messageId);
            if (message == null)
            {
                throw new KeyNotFoundException("Message không tìm thấy");
            }

            message.Status = status;
            await _unitOfWork.SaveChangesAsync();
            return message;
        }

        public async Task DeleteMessageByIdAsync(Guid messageId)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(messageId);
            if (message == null)
            {
                throw new KeyNotFoundException("Message không tìm thấy");
            }

            await _unitOfWork.MessageRepository.DeleteAsync(message);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
