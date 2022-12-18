﻿using AutoMapper;
using Community.API.Dtos.Customer.Authentication;
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
            CreateMap<CustomerSignInRequestDto, Customer>();
            CreateMap<CustomerSignUpRequestDto, Customer>();
            CreateMap<Customer, CustomerAuthenticationResponseDto>();

            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();

            // DTOs - system
            CreateMap<HttpException, HttpExceptionDto>();
            CreateMap<HttpExceptionMessage, HttpExceptionMessageDto>();

            // E2E
            CreateMap<Employee, Employee>()
                .ForMember(e => e.Id, opt => opt.Ignore());

            CreateMap<Customer, Customer>()
                .ForMember(e => e.Id, opt => opt.Ignore());

            CreateMap<Customer, Employee>();
        }
    }
}
