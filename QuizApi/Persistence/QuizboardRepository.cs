using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizApi.Entities;
using QuizApi.Models;
using Answer = QuizApi.Models.Answer;

namespace QuizApi.Persistence;

public class QuizboardRepository: IQuizboardRepository
{
    private readonly IMongoCollection<Quizboard> _quizboardsCollection;

    public QuizboardRepository(IOptions<QuizboardsDatabaseSettings> quizboardDatabaseSettings)
    {
        var mongoClient = new MongoClient(quizboardDatabaseSettings.Value.ConnectionString);
        
        var mongoDatabase = mongoClient.GetDatabase(quizboardDatabaseSettings.Value.DatabaseName);
        
        _quizboardsCollection = mongoDatabase.GetCollection<Quizboard>(quizboardDatabaseSettings.Value.QuizboardsCollectionName);
    }

    public async Task<List<QuizboardDto>> GetAllQuizboards()
    {
        var x = await _quizboardsCollection.Find(qb => true).ToListAsync();
        if (x is null) return [];

        var collectionToDto = x.Select(entry => new QuizboardDto
        {
            Id = entry.Id,
            QuizboardTitle = entry.QuizboardTitle,
            QuizboardDescription = entry.QuizboardDescription,
            EditorIds = [1,2,3],
            Categories = entry.Categories,
            ValuesAscending = entry.ValuesAscending,
            Gamecards = entry.Gamecards?.Select(x =>  new GamecardDto
            {
                QuizboardId = entry.Id ??
                              throw new ArgumentNullException(entry.Id,
                                  "QuizboardId darf nicht null sein."),
                CategoryId = x.CategoryId,
                ValueId = x.ValueId,
                GameMode = x.GameMode,
                EyecatcherTitle = x.EyecatcherTitle,
                QuestionText = x.QuestionText,
                Clues = null,
                OptionalClue = x.OptionalClue,
                PossibleAnswers = x.PossibleAnswers is not null 
                    ? new PossibleAnswer
                        {
                            CorrectAnswers = x.PossibleAnswers.CorrectAnswers?.Select(ca => new Answer
                            {
                                TextAnswer = ca.TextAnswer,
                                ImageLink = ca.ImageLink,
                                SoundLink = ca.SoundLink,
                                Explanation = ca.Explanation,
                            }).ToList(),
                            WrongAnswers = null,
                            AreClickable = false
                        } 
                    : null,

            }).ToList()
        });
        
        return  collectionToDto.ToList();
    }

    public async Task<QuizboardDto?> GetQuizboardById(string quizboardId)
    {
        var filter = Builders<Quizboard>.Filter.Where(qb => qb.Id == quizboardId);
        var document = await _quizboardsCollection.Find(filter).FirstOrDefaultAsync();
        
        return TransformToDt(document);
    }

    public async Task<QuizboardDto> UpdateQuizboard(QuizboardDto quizboard)
    {
        throw new NotImplementedException();
    }

    private static QuizboardDto TransformToDt(Quizboard quizboardFromMongodb)
    {
        var x = new QuizboardDto
        {
            Id = quizboardFromMongodb.Id,
            QuizboardTitle = quizboardFromMongodb.QuizboardTitle,
            QuizboardDescription = quizboardFromMongodb.QuizboardDescription,
            EditorIds = [],
            Categories = quizboardFromMongodb.Categories,
            ValuesAscending = quizboardFromMongodb.ValuesAscending,
            Gamecards = quizboardFromMongodb.Gamecards?.Select(x =>  new GamecardDto
            {
                QuizboardId = quizboardFromMongodb.Id ??
                              throw new ArgumentNullException(quizboardFromMongodb.Id,
                                  "QuizboardId darf nicht null sein."),
                CategoryId = x.CategoryId,
                ValueId = x.ValueId,
                GameMode = x.GameMode,
                EyecatcherTitle = x.EyecatcherTitle,
                QuestionText = x.QuestionText,
                Clues = null,
                OptionalClue = x.OptionalClue,
                PossibleAnswers = x.PossibleAnswers is not null 
                    ? new PossibleAnswer
                    {
                        CorrectAnswers = x.PossibleAnswers.CorrectAnswers?.Select(ca => new Answer
                        {
                            TextAnswer = ca.TextAnswer,
                            ImageLink = ca.ImageLink,
                            SoundLink = ca.SoundLink,
                            Explanation = ca.Explanation,
                        }).ToList(),
                        WrongAnswers = null,
                        AreClickable = false
                    } 
                    : null,

            }).ToList()
        };

        return x;
    }
}