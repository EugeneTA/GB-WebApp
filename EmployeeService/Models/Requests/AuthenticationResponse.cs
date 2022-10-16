using EmployeeService.Models.Dto;
using Microsoft.AspNetCore.Components.Authorization;

namespace EmployeeService.Models.Requests
{
    public class AuthenticationResponse
    {
        public AuthenticationStatus Status { get; set; }

        public SessionDto Session { get; set; }
    }
}
