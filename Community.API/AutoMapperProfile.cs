using AutoMapper;
using Community.API.Dtos.Staff.Authorization;
using Community.API.Dtos.Staff.Role;
using Community.API.Dtos.System;
using Community.API.Utilities.Exceptions;
using Community.Domain.Models;

namespace Community.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // DTOs - business
            CreateMap<StaffSignInRequestDto, Staff>();
            CreateMap<StaffSignUpRequestDto, Staff>();
            CreateMap<Staff, StaffAuthorizationResponseDto>();

            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();

            // DTOs - system
            CreateMap<HttpException, HttpExceptionDto>();
            CreateMap<HttpExceptionMessage, HttpExceptionMessageDto>();

            // E2E
            CreateMap<Staff, Staff>()
                .ForMember(e => e.Id, opt => opt.Ignore());
        }
    }
}
