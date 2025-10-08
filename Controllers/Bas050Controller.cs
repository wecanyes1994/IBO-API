// Controllers/Bas050Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Bas050Controller : TableControllerBase
{
    protected override string TableName => "bas050";
    public Bas050Controller(CrudService svc) : base(svc) { }
}
