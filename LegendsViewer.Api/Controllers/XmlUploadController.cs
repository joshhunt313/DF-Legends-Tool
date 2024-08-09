using System.Xml.Linq;
using LegendsViewer.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace LegendsViewer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class XmlUploadController : ControllerBase
{
    private readonly ILogger<XmlUploadController> _logger;
    private XDocument? _legends;
    private XDocument? _legendsPlus;
    
    public XmlUploadController(ILogger<XmlUploadController> logger)
    {
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadXmlFile(IFormFile legends, IFormFile legendsPlus)
    {
        try
        {
            await ValidateXmlFiles((legends, legendsPlus));
        }
        catch (Exception e)
        {
            return BadRequest("Error while validating XML files: " + e.Message);
        }

        try
        {
            ParseXmlFiles();
        }
        catch (Exception e)
        {
            return BadRequest("Error while parsing XML files: " + e.Message);
        }
        
        return Ok("Files uploaded successfully");
    }
    
    private async Task ValidateXmlFiles((IFormFile legends, IFormFile legendsPlus) files)
    {
        var tasks = new List<Task>
        {
            ValidateXmlFile(files.legends),
            ValidateXmlFile(files.legendsPlus)
        };

        await Task.WhenAll(tasks);
    }
    
    private async Task ValidateXmlFile(IFormFile file)
    {
        try
        {
            if (file.FileName.Contains("plus"))
            {
                _legendsPlus = await XDocument.LoadAsync(file.OpenReadStream(), LoadOptions.None, CancellationToken.None);
            }
            else
            {
                _legends = await XDocument.LoadAsync(file.OpenReadStream(), LoadOptions.None, CancellationToken.None);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error while validating XML file: {}", e.Message);
            throw;
        }
    }
    
    private void ParseXmlFiles()
    {
        try
        {
            var legends = _legends?.Root;
            var legendsPlus = _legendsPlus?.Root;
            
            if (legends != null && legendsPlus != null)
            {
                var world = DfWorld.FromXml(legends, legendsPlus);
            }
            else
            {
                throw new ArgumentNullException(nameof(legends), "XML files are required");
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error while parsing XML files: {}", e.Message);
            throw;
        }
    }
}