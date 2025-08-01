using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizApi.Entities;

public class Question
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("text")]
    public required string QuestionText { get; set; }
    
    [BsonElement("type")]
    public required List<string> QuestionType { get; set; } = [];

    [BsonElement("correctAnswers")]
    public List<string> CorrectAnswers { get; set; } = [];

    [BsonElement("wrongAnswers")]
    public List<string> WrongAnswers { get; set; } = [];

    [BsonElement("categories")]
    public List<string> QuestionCategories { get; set; } = [];
}