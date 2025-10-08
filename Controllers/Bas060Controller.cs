// Controllers/Bas060Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Bas060Controller : TableControllerBase
{
    protected override string TableName => "bas060";
    public Bas060Controller(CrudService svc) : base(svc) { }
}
