// Controllers/Sal022Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Sal022Controller : TableControllerBase
{
    protected override string TableName => "sal022";
    public Sal022Controller(CrudService svc) : base(svc) { }
}
