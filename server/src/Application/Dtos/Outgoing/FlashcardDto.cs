using StudyZen.Domain.Entities;

namespace StudyZen.Application.Dtos;

public class FlashcardDto 
{
    public int Id { get; set; }
    public int FlashcardSetId { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }

    public FlashcardDto(Flashcard flashcard)
    {
        Id = flashcard.Id;
        FlashcardSetId = flashcard.FlashcardSetId;
        Question = flashcard.Question;
        Answer = flashcard.Answer;
    }

    public static FlashcardDto ToDto(Flashcard flashcard)
    {
        return new FlashcardDto(flashcard);
    }

}