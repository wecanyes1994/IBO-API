// Controllers/Bas061Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Bas061Controller : TableControllerBase
{
    protected override string TableName => "bas061";
    public Bas061Controller(CrudService svc) : base(svc) { }
}
