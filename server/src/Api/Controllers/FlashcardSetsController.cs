using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using FluentValidation;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class FlashcardSetsController : ControllerBase
{
    private readonly IFlashcardSetService _flashcardSetService;
    private readonly IValidator<CreateFlashcardSetDto> _createFlashcardSetValidator;
    private readonly IValidator<UpdateFlashcardSetDto> _updateFlashcardSetValidator;

    public FlashcardSetsController(IFlashcardSetService flashcardSetService, IValidator<CreateFlashcardSetDto> createFlashcardSetValidator, IValidator<UpdateFlashcardSetDto> updateFlashcardSetValidator)
    {
        _flashcardSetService = flashcardSetService;
        _createFlashcardSetValidator = createFlashcardSetValidator;
        _updateFlashcardSetValidator = updateFlashcardSetValidator;
    }

    [HttpPost]
    public IActionResult CreateFlashcardSet([FromBody] CreateFlashcardSetDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var createdFlashcardSet = _flashcardSetService.CreateFlashcardSet(request);
        return CreatedAtAction(nameof(GetFlashcardSet), new { flashcardSetId = createdFlashcardSet.Id }, createdFlashcardSet);
    }

    [HttpGet("{flashcardSetId}")]
    public IActionResult GetFlashcardSet(int flashcardSetId)
    {
        var flashcardSet = _flashcardSetService.GetFlashcardSetById(flashcardSetId);
        return flashcardSet == null ? NotFound() : Ok(flashcardSet);
    }

    [HttpGet]
    [Route("all")]
    public IActionResult GetAllFlashcardSets()
    {
        return Ok(_flashcardSetService.GetAllFlashcardSets());
    }

    [HttpGet]
    public IActionResult GetFlashcardSetsByLectureId(int lectureId)
    {
        return Ok(_flashcardSetService.GetFlashcardSetsByLectureId(lectureId));
    }

    [HttpPatch("{flashcardSetId}")]
    public IActionResult UpdateFlashcardSetById(int flashcardSetId, [FromBody] UpdateFlashcardSetDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var isSuccess = _flashcardSetService.UpdateFlashcardSet(flashcardSetId, request);
        return isSuccess ? Ok() : BadRequest();
    }

    [HttpDelete("{flashcardSetId}")]
    public IActionResult DeleteFlashcardSet(int flashcardSetId)
    {
        var isSuccess = _flashcardSetService.DeleteFlashcardSet(flashcardSetId);
        return isSuccess ? Ok() : NotFound();
    }
}