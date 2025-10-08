// Controllers/Bas030Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Bas030Controller : TableControllerBase
{
    protected override string TableName => "bas030";
    public Bas030Controller(CrudService svc) : base(svc) { }
}
