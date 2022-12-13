using System.Text.Json.Serialization;
using static Community.Domain.Models.Permission;

namespace Community.API.Dtos.Employee.Permission
{
    public class PermissionResponseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResourceType Resource { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OperationType Operation { get; set; }
    }
}
