using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;
using FluentValidation;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class FlashcardsController : ControllerBase
{
    private readonly IFlashcardService _flashcardService;
    private readonly IValidator<CreateFlashcardDto> _createFlashcardValidator;
    private readonly IValidator<UpdateFlashcardDto> _updateFlashcardValidator;

    public FlashcardsController(IFlashcardService flashcardService, IValidator<CreateFlashcardDto> createFlashcardValidator, IValidator<UpdateFlashcardDto> updateFlashcardValidator)
    {
        _flashcardService = flashcardService;
        _createFlashcardValidator = createFlashcardValidator;
        _updateFlashcardValidator = updateFlashcardValidator;
    }

    [HttpPost]
    public IActionResult CreateFlashcard([FromBody] CreateFlashcardDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var newFlashcard = _flashcardService.CreateFlashcard(request);
        return CreatedAtAction(nameof(GetFlashcard), new { flashcardId = newFlashcard.Id }, newFlashcard);
    }

    [HttpGet]
    [Route("{flashcardId}")]
    public IActionResult GetFlashcard(int flashcardId)
    {
        var flashcard = _flashcardService.GetFlashcardById(flashcardId);
        return flashcard is null ? NotFound() : Ok(flashcard);
    }

    [HttpGet]
    public IActionResult GetFlashcardsBySetId(int flashcardSetId)
    {
        return Ok(_flashcardService.GetFlashcardsBySetId(flashcardSetId));
    }

    [HttpPatch]
    [Route("{flashcardId}")]
    public IActionResult UpdateFlashcardById(int flashcardId, [FromBody] UpdateFlashcardDto? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var isSuccess = _flashcardService.UpdateFlashcard(flashcardId, request);
        return isSuccess ? Ok() : BadRequest();
    }


    [HttpDelete("{flashcardId}")]
    public IActionResult DeleteFlashcard(int flashcardId)
    {
        var isSuccess = _flashcardService.DeleteFlashcard(flashcardId);
        return isSuccess ? Ok() : NotFound();
    }
}