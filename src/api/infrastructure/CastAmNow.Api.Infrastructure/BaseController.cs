using AutoMapper;
using CastAmNow.Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CastAmNow.Api.Infrastructure
{
    public abstract class BaseController(IHttpContextAccessor httpContextAccessor,IMapper mapper) : ControllerBase
    {
        public string? ClientIp => httpContextAccessor.HttpContext.GetClientIp();
    }
}
