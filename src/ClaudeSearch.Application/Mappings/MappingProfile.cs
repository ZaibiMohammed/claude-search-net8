using AutoMapper;
using ClaudeSearch.Domain.Entities;
using ClaudeSearch.Application.DTOs;

namespace ClaudeSearch.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SearchResult, SearchResultDto>();
        }
    }
}