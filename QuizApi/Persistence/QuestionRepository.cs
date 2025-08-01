using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuizApi.Entities;
using QuizApi.Models;

namespace QuizApi.Persistence;

public class QuestionRepository : IQuestionRepository
{
    private readonly IMongoCollection<Question>  _questionsCollection;

    public QuestionRepository(IOptions<QuestionCatalogueDatabaseSettings> questionCatalogueDatabaseSettings)
    {
        var mongoClient = new MongoClient(questionCatalogueDatabaseSettings.Value.ConnectionString);
        
        var mongoDatabase = mongoClient.GetDatabase(questionCatalogueDatabaseSettings.Value.DatabaseName);
        
        _questionsCollection = mongoDatabase.GetCollection<Question>(questionCatalogueDatabaseSettings.Value.QuestionsCollectionName);
    }

    public async Task<List<Question>> GetQuestions() => 
        await _questionsCollection.Find(q => true).ToListAsync();
    
    
}