using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizApi.Entities;

public class Quizboard
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("title")]
    public string? QuizboardTitle { get; set; }
    
    [BsonElement("description")]
    public string? QuizboardDescription { get; set; }
    
    [BsonElement("editors")]
    public List<string>? EditorUsernames { get; set; }
    
    /// <summary>
    /// All possible categories 
    /// </summary>
    [BsonElement("categories")]
    public string[]? Categories { get; set; }
    
    /// <summary>
    /// All possible values a question card can have in ascending order
    /// </summary>
    [BsonElement("valuesAscending")]
    public int[]? ValuesAscending { get; set; }
    
    /// <summary>
    /// A gamecard contains a question with all the related data (value, answers, media, etc.) for the game.
    /// </summary>
    [BsonElement("gamecards")]
    public List<Gamecard>? Gamecards { get; set; }
}

