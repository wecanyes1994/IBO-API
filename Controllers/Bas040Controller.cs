// Controllers/Bas040Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Bas040Controller : TableControllerBase
{
    protected override string TableName => "bas040";
    public Bas040Controller(CrudService svc) : base(svc) { }
}
