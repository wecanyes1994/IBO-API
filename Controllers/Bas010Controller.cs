// Controllers/Bas010Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Bas010Controller : TableControllerBase
{
    protected override string TableName => "bas010";
    public Bas010Controller(CrudService svc) : base(svc) { }
}
