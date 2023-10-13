using System.Collections.Concurrent;
using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services
{
    public class FlashcardImporter
    {
        private readonly IFlashcardService _flashcardService;

        public FlashcardFileImporter(IFlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
        }

        public void ImportFlashcardsFromCsvStream(Stream stream, int flashcardSetId)
        {
            try
            {
                var lines = ReadCsvLinesFromStream(stream);

                var flashcardsToCreate = new BlockingCollection<CreateFlashcardDto>();

                Parallel.ForEach(lines, line =>
                {
                    var values = line.Split(',');

                    if (values.Length == 2)
                    {
                        string front = values[0];
                        string answer = values[1];

                        var createFlashcardDto = new CreateFlashcardDto(flashcardSetId, question, answer);

                        flashcardsToCreate.Add(createFlashcardDto);
                    }
                });

                
                flashcardsToCreate.CompleteAdding();

                
                var createdFlashcards = _flashcardService.CreateFlashcardsCollection(flashcardsToCreate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing flashcards: {ex.Message}");
            }
        }

        private List<string> ReadCsvLinesFromStream(Stream stream)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }
    }
}