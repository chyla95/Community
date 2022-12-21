using AutoMapper;
using Community.API.Dtos.Role;
using Community.API.Filters;
using Community.API.Utilities.Exceptions;
using Community.Domain.Models;
using Community.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using static Community.Domain.Models.Employee;

namespace Community.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public RoleController(IMapper mapper, IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponseDto>>> GetMany()
        {
            IEnumerable<Role> roles = await _roleService.GetManyAsync();

            IEnumerable<RoleResponseDto> response = _mapper.Map<IEnumerable<RoleResponseDto>>(roles);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleResponseDto>> Get(int id)
        {
            Role? role = await _roleService.GetAsync(id);
            if (role == null) throw new HttpNotFoundException("Role not found!");

            RoleResponseDto response = _mapper.Map<RoleResponseDto>(role);
            return Ok(response);
        }

        [HttpPost]
        [EmployeeAuthorization(Permission.ManageRoles)]
        public async Task<ActionResult<RoleResponseDto>> Add(RoleRequestDto roleRequestDto)
        {
            bool isNameTaken = await _roleService.IsNameTakenAsync(roleRequestDto.Name);
            if (isNameTaken) throw new HttpBadRequestException("Name is Taken!");

            Role role = _mapper.Map<Role>(roleRequestDto);
            await _roleService.AddAsync(role);

            return Ok();
        }

        [HttpPut("{id}")]
        [EmployeeAuthorization(Permission.ManageRoles)]
        public async Task<ActionResult<RoleResponseDto>> Update(int id, RoleRequestDto roleRequestDto)
        {
            Role? role = await _roleService.GetAsync(id);
            if (role == null) throw new HttpNotFoundException("Role not found!");

            bool isNameTaken = await _roleService.IsNameTakenAsync(roleRequestDto.Name, id);
            if (isNameTaken) throw new HttpBadRequestException("Name is taken!");

            _mapper.Map(roleRequestDto, role);
            await _roleService.UpdateAsync(role);

            return Ok();
        }

        [HttpDelete("{id}")]
        [EmployeeAuthorization(Permission.ManageRoles)]
        public async Task<ActionResult> Remove(int id)
        {
            Role? role = await _roleService.GetAsync(id);
            if (role == null) throw new HttpNotFoundException("Role not found!");

            await _roleService.RemoveAsync(role);
            return Ok();
        }
    }
}
