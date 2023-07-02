using System.Net;
using dotnet_Api.DTOs.Account;
using dotnet_Api.Entities;
using dotnet_Api.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register(RegisterRequest registerRequest)
        {
            var account = registerRequest.Adapt<Account>();
            await _accountService.Register(account);

            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            var account = await _accountService.Login(loginRequest.Username, loginRequest.Password);

            if (account == null) return Unauthorized();

            return Ok(new { token = _accountService.GenerateToken(account) });
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> Info()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            if (accessToken == null) return Unauthorized();

            var account = _accountService.GetInfo(accessToken);

            return Ok(new
            {
                username = account.Username,
                role = account.Role.Name
            });
        }
    }
}