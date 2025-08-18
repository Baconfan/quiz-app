using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using QuizApi.Dtos.Input;
using QuizApi.Enums;
using Entity = QuizApi.Entities;
using Model = QuizApi.Models;

namespace QuizApi.Persistence;

public class QuizboardRepository(
    IMongoDatabase database,
    IOptions<Model.QuizboardsDatabaseSettings> quizboardDatabaseSettings)
    : IQuizboardRepository
{
    private readonly IMongoCollection<Entity.Quizboard> _quizboardsCollection = database.GetCollection<Entity.Quizboard>(quizboardDatabaseSettings.Value.QuizboardsCollectionName);

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

    public async Task UpsertQuizboardCategory(UpdateQuizboardCategoryDto dto)
    {
        var filter = Builders<Entity.Quizboard>.Filter.Where(qb => qb.Id == dto.QuizboardId);
        var update = Builders<Entity.Quizboard>.Update.Set(qb => qb.Categories![dto.CategoryId], dto.NewCategoryName);

        await _quizboardsCollection.UpdateOneAsync(filter, update, new  UpdateOptions { IsUpsert = true });
    }

    public async Task DeleteQuizboardCategory(string quizboardId, int categoryId)
    {
        
        
        // renew index of existing cards
        throw new NotImplementedException();
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

    public async Task UpsertQuizcard(Model.GamecardDto dto)
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

        var cardForUpsert = TransformToGamecardDocument(dto);
        
        var update = Builders<Entity.Quizboard>.Update.Set(qb => qb.Gamecards.AllMatchingElements("gamecards"), cardForUpsert);
        
        var updateOptions = new UpdateOptions { ArrayFilters = arrayFilters, IsUpsert = true};
        
        var updateResult = await _quizboardsCollection.UpdateOneAsync(filter, update, updateOptions);

        if (updateResult.ModifiedCount == 0)
        {
            var pushNew = Builders<Entity.Quizboard>.Update.Push(q => q.Gamecards, cardForUpsert);
            
            await _quizboardsCollection.UpdateOneAsync(filter, pushNew.SetOnInsert(q => q.Id, dto.QuizboardId));
        }
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
        
        await _quizboardsCollection.UpdateOneAsync(filter, updateDefinition);
    }

    public async Task AddImageToQuizcard(Model.AddQuizImageToQuizcardModel imageReference)
    {
        var filter = Builders<Entity.Quizboard>.Filter.Where(qb => qb.Id == imageReference.QuizcardId.QuizboardId);

        var arrayFilters = new[]
        {
            new BsonDocumentArrayFilterDefinition<BsonDocument>(
                new BsonDocument("gc.categoryId", imageReference.QuizcardId.CategoryId)
                    .Add("gc.valueId", imageReference.QuizcardId.ValueId))
        };
        
        await  _quizboardsCollection.UpdateOneAsync(filter, GetUpdateDefinition(imageReference), new UpdateOptions { ArrayFilters = arrayFilters});
    }

    public async Task DeleteImageFromQuizcard()
    {
        throw new NotImplementedException();
    }

    private static UpdateDefinition<Entity.Quizboard> GetUpdateDefinition(Model.AddQuizImageToQuizcardModel imageReference)
    {
        switch (imageReference.Assignment)
        {
            case ImageAssignment.CorrectAnswer:
            {
                var fieldPath = $"gamecards.$[gc].possibleAnswers.correctAnswers.{imageReference.Position}.answerImageUrl";

                return Builders<Entity.Quizboard>.Update.Set(fieldPath, imageReference.Url);
            }
            case ImageAssignment.WrongAnswer:
            {
                var fieldPath = $"gamecards.$[gc].possibleAnswers.wrongAnswers.{imageReference.Position}.answerImageUrl";

                return Builders<Entity.Quizboard>.Update.Set(fieldPath, imageReference.Url);
            }
            default:
            {
                // Image for Question
                var newQuizcardImage = new Entity.CardImage
                {
                    ImageUrl = imageReference.Url,
                    PositionNumber = null
                };
            
                return Builders<Entity.Quizboard>.Update.Push(
                    "gamecards.$[gc].questionImages",
                    newQuizcardImage
                );
            }
        }
    }

    private static Model.QuizboardDto TransformToDt(Entity.Quizboard quizboardFromMongodb)
    {
        var outputModel = new Model.QuizboardDto
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
                IsMultipleChoice = x.IsMultipleChoice,
                EyecatcherTitle = x.EyecatcherTitle ?? "",
                QuestionText = x.QuestionText,
                QuestionImages = x.QuestionImages?.Select(TransformToCardImageModel).ToList() ?? [],
                Clues = x.Clues ?? [],
                OptionalClue = x.OptionalClue ?? "",
                PossibleAnswers = x.PossibleAnswers is not null 
                    ? new Model.PossibleAnswers
                    {
                        CorrectAnswers = x.PossibleAnswers.CorrectAnswers?.Select(TransformAnswers)
                                             .ToList() ?? [],
                        CorrectAnswersTextList = x.PossibleAnswers.CorrectAnswersTextList ?? [],
                        WrongAnswers = x.PossibleAnswers.WrongAnswers?.Select(TransformAnswers)
                                           .ToList() ?? [],
                        AreClickable = x.PossibleAnswers.AreClickable
                    }
                    : new Model.PossibleAnswers()
            }).ToList()
        };

        return outputModel;
    }

    private static Model.Answer TransformAnswers(Entity.Answer entity) =>
        new()
        {
            PositionNumber = entity.PositionNumber,
            TextAnswer = entity.TextAnswer,
            ImageUrl = entity.ImageUrl ?? "",
            SoundUrl = entity.SoundUrl ?? "",
            Explanation = entity.Explanation ?? ""
        };

    private static Model.CardImage TransformToCardImageModel(Entity.CardImage cardImageFromMongodb) =>
        new()
        {
            ImageUrl = cardImageFromMongodb.ImageUrl,
            PositionNumber = cardImageFromMongodb.PositionNumber
        };

    private static Entity.CardImage TransformToCardImageDocument(Model.CardImage dto) =>
        new()
        {
            ImageUrl = dto.ImageUrl,
            PositionNumber = dto.PositionNumber
        };

    private static Entity.Gamecard TransformToGamecardDocument(Model.GamecardDto dto)
    {
        return new Entity.Gamecard
        {
            CategoryId = dto.CategoryId,
            ValueId = dto.ValueId,
            GameMode = dto.GameMode,
            IsMultipleChoice = dto.IsMultipleChoice,
            QuestionImages = dto.QuestionImages.Select(TransformToCardImageDocument).ToList(),
            EyecatcherTitle = dto.EyecatcherTitle,
            QuestionText = dto.QuestionText,
            Clues = dto.Clues.Count != 0
                ? dto.Clues.ToList()
                : null,
            OptionalClue = dto.OptionalClue,
            PossibleAnswers =
                new Entity.PossibleAnswers
                {
                    CorrectAnswers = dto.PossibleAnswers.CorrectAnswers.Select(TransformToAnswerDocument)
                        .ToList(),
                    CorrectAnswersTextList = dto.PossibleAnswers.CorrectAnswersTextList,
                    WrongAnswers = dto.PossibleAnswers.WrongAnswers.Select(TransformToAnswerDocument)
                        .ToList(),
                    AreClickable = dto.PossibleAnswers.AreClickable,
                }
        };
    }

    private static Entity.Answer TransformToAnswerDocument(Model.Answer dto)
    {
        return new Entity.Answer
        {
            PositionNumber = dto.PositionNumber,
            TextAnswer = dto.TextAnswer,
            ImageUrl = dto.ImageUrl,
            SoundUrl = dto.SoundUrl,
            Explanation = dto.Explanation
        };
    }
}