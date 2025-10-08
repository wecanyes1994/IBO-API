// Controllers/Sal020Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Sal020Controller : TableControllerBase
{
    protected override string TableName => "sal020";
    public Sal020Controller(CrudService svc) : base(svc) { }
}
