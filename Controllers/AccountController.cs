using System.Net;
using dotnet_Api.DTOs.Account;
using dotnet_Api.Entities;
using dotnet_Api.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountSerivce _accountSerive;

        public AccountController(IAccountSerivce accountSerive) => _accountSerive = accountSerive;

        [HttpPost("[action]")]
        public async Task<ActionResult> Register(RegisterRequest registerRequest)
        {
            var account = registerRequest.Adapt<Account>();
            await _accountSerive.Register(account);

            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}