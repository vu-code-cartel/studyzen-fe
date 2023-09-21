using Microsoft.AspNetCore.Mvc;
using StudyZen.Common;
using StudyZen.FlashCards.Requests;


namespace StudyZen.FlashCards;

[ApiController]
[Route("[controller]")]
public sealed class FlashCardsController : ControllerBase
{
    private readonly IFlashCardService _flashCardService;

    public FlashCardsController(IFlashCardService flashCardService)
    {
        _flashCardService = flashCardService;
    }

    [HttpPost]
    public IActionResult CreateFlashCard([FromBody] CreateFlashCardRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var flashCard = _flashCardService.AddFlashCard(request);
        return CreatedAtAction(nameof(GetFlashCard), new { flashCardId = flashCard.Id }, flashCard);
    }

    [HttpGet]
    [Route("{flashCardId}")]
    public IActionResult GetFlashCard(int flashCardId)
    {
        var flashCard = _flashCardService.GetFlashCardById(flashCardId);

        return flashCard == null ? NotFound() : Ok(flashCard);
    }


    [HttpGet]
    public IActionResult GetFlashCardsBySetId(int? flashCardSetId)
    {
        return Ok(_flashCardService.GetFlashCardsBySetId(flashCardSetId));
    }


    [HttpDelete("{flashCardId}")]
    public IActionResult DeleteFlashcard(int flashCardId)
    {
        _flashCardService.DeleteFlashCardById(flashCardId);
        return NoContent();
    }


    [HttpPatch]
    [Route("{flashCardId}")]
    public IActionResult UpdateFlashCardById(int flashCardId, [FromBody] UpdateFlashCardRequest? request)
    {
        request = request.ThrowIfRequestArgumentNull(nameof(request));
        var updatedFlashCard = _flashCardService.UpdateFlashCardById(flashCardId, request);
        return updatedFlashCard == null ? NotFound() : Ok(updatedFlashCard);
    }
    [HttpGet]
    public IActionResult GetAllFlashCards()
    {
        return Ok(_flashCardService.GetAllFlashCards());
    }


}
