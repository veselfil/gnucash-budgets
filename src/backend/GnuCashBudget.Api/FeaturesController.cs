using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace GnuCashBudget.Api;

[ApiController]
[Route("features")]
public class FeaturesController: ControllerBase
{
    private readonly ApplicationPartManager _partManager;

    public FeaturesController(ApplicationPartManager partManager)
    {
        _partManager = partManager;
    }

    [HttpGet]
    public ActionResult Index()
    {
        var controllerFeature = new ControllerFeature();
        _partManager.PopulateFeature(controllerFeature);
        return Ok(new { Contorllers = controllerFeature.Controllers.ToList() });
    }
}