// Controllers/Sal021Controller.cs
using IBOWebAPI.Controllers.Base;
using IBOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBOWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class Sal021Controller : TableControllerBase
{
    protected override string TableName => "sal021";
    public Sal021Controller(CrudService svc) : base(svc) { }
}
