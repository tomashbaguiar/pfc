using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Template.Api.Services;

namespace Template.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TemplateController : ControllerBase
{
    private readonly ILogger<TemplateController> _logger;
    private readonly TemplateService _service;

    public TemplateController(
        ILogger<TemplateController> logger,
        TemplateService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<object?>> Get([FromQuery] string name)
    {
        _logger.LogInformation("[GET] {Name}", name);

        Models.Template? template = await _service.GetAsync(name);

        if (template is null) return NotFound();

        _logger.LogInformation("Rule: {Rule}", template?.Rule);

        return Ok(template?.Rule);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromQuery] string name,
        [FromForm] string rule)
    {
        _logger.LogInformation("[POST] {Name}: {Rule}", name, rule);

        Models.Template? template = await _service.GetAsync(name);

        if (template is not null) return BadRequest();

        var newTemplate = new Models.Template
        {
            Name = name,
            Rule = rule,
            Id = ObjectId.GenerateNewId().ToString(),
        };

        await _service.CreateAsync(newTemplate);

        _logger.LogInformation("Template: {@Template}", newTemplate);

        return CreatedAtAction(
            nameof(Post),
            new { name = newTemplate.Name },
            template);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromQuery] string name,
        [FromForm] string rule)
    {
        _logger.LogInformation("[PUT] {Name}: {Rule}", name, rule);

        Models.Template? template = await _service.GetAsync(name);

        if (template is null) return NotFound();

        var updatedTemplate = template;
        updatedTemplate.Rule = rule;

        _logger.LogInformation("Template: {@Template}", updatedTemplate);

        await _service.UpdateAsync(name, updatedTemplate);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string name)
    {
        _logger.LogInformation("[DELETE] {Name}", name);

        Models.Template? template = await _service.GetAsync(name);

        _logger.LogInformation("Template: {@Template}", template);

        if (template is null) return NotFound();

        await _service.RemoveAsync(name);

        return NoContent();
    }
}