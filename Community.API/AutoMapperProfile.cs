using AutoMapper;
using Community.API.Dtos.Employee.Authorization;
using Community.API.Dtos.Employee.Role;
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
            CreateMap<EmployeeSignInRequestDto, Employee>();
            CreateMap<EmployeeSignUpRequestDto, Employee>();
            CreateMap<Employee, EmployeeAuthorizationResponseDto>();

            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();

            // DTOs - system
            CreateMap<HttpException, HttpExceptionDto>();
            CreateMap<HttpExceptionMessage, HttpExceptionMessageDto>();

            // E2E
            CreateMap<Employee, Employee>()
                .ForMember(e => e.Id, opt => opt.Ignore());
        }
    }
}
