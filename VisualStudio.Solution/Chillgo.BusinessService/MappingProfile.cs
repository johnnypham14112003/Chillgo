using AutoMapper;
using Chillgo.BusinessService.SharedDTOs;
using Chillgo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chillgo.BusinessService
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create map from the entity to the DTO
            CreateMap<Conversation, ConversationDto>();
        }
    }
}
