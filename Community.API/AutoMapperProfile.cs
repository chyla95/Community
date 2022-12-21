using AutoMapper;
using Community.API.Dtos.Authentication;
using Community.API.Dtos.Customer;
using Community.API.Dtos.Employee;
using Community.API.Dtos.Role;
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

            CreateMap<CustomerRequestDto, Customer>();
            CreateMap<Customer, CustomerResponseDto>();

            CreateMap<EmployeeRequestDto, Employee>();
            CreateMap<Employee, EmployeeResponseDto>();

            CreateMap<RoleRequestDto, Role>();
            CreateMap<Role, RoleResponseDto>();

            // E2E
            CreateMap<Customer, Employee>();
        }
    }
}
