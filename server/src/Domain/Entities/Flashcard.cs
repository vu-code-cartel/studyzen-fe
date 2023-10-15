using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyZen.Domain.Constraints;


namespace StudyZen.Domain.Entities;

public sealed class Flashcard : BaseEntity
{
    public int FlashcardSetId { get; set; }

    [Required]
    [StringLength(FlashcardConstraints.FrontMaxLength)]
    public string Front { get; set; }

    [Required]
    [StringLength(FlashcardConstraints.BackMaxLength)]
    public string Back { get; set; }

    public Flashcard(int flashcardSetId, string front, string back) : base(default)
    {
        FlashcardSetId = flashcardSetId;
        Front = front;
        Back = back;
    }
}