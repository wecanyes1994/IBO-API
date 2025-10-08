// Controllers/Bas110Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Bas110Controller : TableControllerBase
{
    protected override string TableName => "bas110";
    public Bas110Controller(CrudService svc) : base(svc) { }
}
