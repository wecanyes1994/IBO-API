// Controllers/Sal023Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Sal023Controller : TableControllerBase
{
    protected override string TableName => "sal023";
    public Sal023Controller(CrudService svc) : base(svc) { }
}
