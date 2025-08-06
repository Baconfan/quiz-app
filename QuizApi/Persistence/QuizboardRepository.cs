using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Entity = QuizApi.Entities;
using QuizApi.InputDto;
using Model = QuizApi.Models;

namespace QuizApi.Persistence;

public class QuizboardRepository: IQuizboardRepository
{
    private readonly IMongoCollection<Entity.Quizboard> _quizboardsCollection;

    public QuizboardRepository(IOptions<Model.QuizboardsDatabaseSettings> quizboardDatabaseSettings)
    {
        var mongoClient = new MongoClient(quizboardDatabaseSettings.Value.ConnectionString);
        
        var mongoDatabase = mongoClient.GetDatabase(quizboardDatabaseSettings.Value.DatabaseName);
        
        _quizboardsCollection = mongoDatabase.GetCollection<Entity.Quizboard>(quizboardDatabaseSettings.Value.QuizboardsCollectionName);
    }

    public async Task<List<Model.QuizboardDto>> GetAllQuizboards()
    {
        var allBoards = await _quizboardsCollection.Find(qb => true).ToListAsync();
        
        return allBoards is null 
            ? [] 
            : allBoards.Select(TransformToDt).ToList();
    }

    public async Task<Model.QuizboardDto?> GetQuizboardById(string quizboardId)
    {
        var filter = Builders<Entity.Quizboard>.Filter.Where(qb => qb.Id == quizboardId);
        var document = await _quizboardsCollection.Find(filter).FirstOrDefaultAsync();

        return document is null 
            ? null 
            : TransformToDt(document);
    }

    public async Task UpdateQuizboardCategories(UpdateQuizboardCategoryDto dto)
    {
        var filter = Builders<Entity.Quizboard>.Filter.Where(qb => qb.Id == dto.QuizboardId);
        var update = Builders<Entity.Quizboard>.Update.Set("categories", dto.NewCategories);

        await _quizboardsCollection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateQuizboardValues(UpdateQuizboardValuesDto dto)
    {
        var filter = Builders<Entity.Quizboard>.Filter.Where(qb => qb.Id == dto.QuizboardId);
        var update = Builders<Entity.Quizboard>.Update.Set("values", dto.NewValues);
        
        await _quizboardsCollection.UpdateOneAsync(filter, update);
    }

    public async Task CreateNewQuizcard(Model.GamecardDto dto)
    {
        var filter = Builders<Entity.Quizboard>.Filter.Where(qb => qb.Id == dto.QuizboardId);
        var update = Builders<Entity.Quizboard>.Update.Push(qb => qb.Gamecards, TransformToGamecardDocument(dto));

        await _quizboardsCollection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateQuizcard(Model.GamecardDto dto)
    {
        var filter = Builders<Entity.Quizboard>.Filter.Where(qb => qb.Id == dto.QuizboardId);

        var arrayFilters = new List<ArrayFilterDefinition>
        {
            new BsonDocumentArrayFilterDefinition<Entity.Quizboard>(
                new BsonDocument
                {
                    { "gamecards.categoryId", dto.CategoryId },
                    { "gamecards.valueId", dto.ValueId }
                })
        };
        
        var update = Builders<Entity.Quizboard>.Update.Set(qb => qb.Gamecards.AllMatchingElements("gamecards"), TransformToGamecardDocument(dto));
        
        var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters };
        
        await _quizboardsCollection.UpdateOneAsync(filter, update, updateOptions);
    }

    public async Task DeleteQuizcardById(DeleteQuizcardDto dto)
    {
        var filter = Builders<Entity.Quizboard>.Filter.Where(qb => qb.Id == dto.QuizboardId);

        var updateDefinition = Builders<Entity.Quizboard>
            .Update
            .PullFilter(
                qb => qb.Gamecards, 
                Builders<Entity.Gamecard>.Filter.And(
                    Builders<Entity.Gamecard>.Filter.Eq(c => c.CategoryId, dto.CategoryId),
                    Builders<Entity.Gamecard>.Filter.Eq(c => c.ValueId,    dto.ValueId)
                )
            );
        
        var result = await _quizboardsCollection.UpdateOneAsync(filter, updateDefinition);
        
        return;
    }   

    private static Model.QuizboardDto TransformToDt(Entity.Quizboard quizboardFromMongodb)
    {
        var x = new Model.QuizboardDto
        {
            Id = quizboardFromMongodb.Id,
            QuizboardTitle = quizboardFromMongodb.QuizboardTitle,
            QuizboardDescription = quizboardFromMongodb.QuizboardDescription,
            EditorIds = [],
            Categories = quizboardFromMongodb.Categories,
            ValuesAscending = quizboardFromMongodb.ValuesAscending,
            Gamecards = quizboardFromMongodb.Gamecards?.Select(x =>  new Model.GamecardDto
            {
                QuizboardId = quizboardFromMongodb.Id ??
                              throw new ArgumentNullException(quizboardFromMongodb.Id,
                                  "QuizboardId darf nicht null sein."),
                CategoryId = x.CategoryId,
                ValueId = x.ValueId,
                GameMode = x.GameMode,
                EyecatcherTitle = x.EyecatcherTitle,
                QuestionText = x.QuestionText,
                Clues = x.Clues?.Select(c => new Model.Clue
                {
                    ClueText = c.ClueText,
                    ClueImageLink = c.ClueImageLink
                }).ToList(),
                OptionalClue = x.OptionalClue,
                PossibleAnswers = x.PossibleAnswers is not null 
                    ? new Model.PossibleAnswer
                    {
                        CorrectAnswers = x.PossibleAnswers.CorrectAnswers?.Select(TransformAnswers).ToList(),
                        WrongAnswers = x.PossibleAnswers.WrongAnswers?.Select(TransformAnswers).ToList(),
                        AreClickable = x.PossibleAnswers.AreClickable
                    } 
                    : null

            }).ToList()
        };

        return x;
    }

    private static Model.Answer TransformAnswers(Entity.Answer entity) =>
        new()
        {
            TextAnswer = entity.TextAnswer,
            ImageLink = entity.ImageLink,
            SoundLink = entity.SoundLink,
            Explanation = entity.Explanation
        };

    private static Entity.Gamecard TransformToGamecardDocument(Model.GamecardDto dto)
    {
        return new Entity.Gamecard
        {
            CategoryId = dto.CategoryId,
            ValueId = dto.ValueId,
            GameMode = dto.GameMode,
            EyecatcherTitle = dto.EyecatcherTitle,
            QuestionText = dto.QuestionText,
            Clues = dto.Clues?.Select(c => new Entity.Clue
            {
                ClueText = c.ClueText,
                ClueImageLink = c.ClueImageLink
            }).ToList(),
            OptionalClue = dto.OptionalClue,
            PossibleAnswers = dto.PossibleAnswers is not null 
                ? new Entity.PossibleAnswers
                {
                    CorrectAnswers = dto.PossibleAnswers.CorrectAnswers?.Select(TransformToAnswerDocument)
                        .ToList(),
                    WrongAnswers = dto.PossibleAnswers.WrongAnswers?.Select(TransformToAnswerDocument).ToList(),
                    AreClickable = dto.PossibleAnswers.AreClickable,
                }
                : null
        };
    }

    private static Entity.Answer TransformToAnswerDocument(Model.Answer dto)
    {
        return new Entity.Answer
        {
            TextAnswer = dto.TextAnswer,
            ImageLink = dto.ImageLink,
            SoundLink = dto.SoundLink,
            Explanation = dto.Explanation
        };
    }
}