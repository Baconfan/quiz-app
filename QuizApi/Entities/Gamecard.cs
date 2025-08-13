using MongoDB.Bson.Serialization.Attributes;

namespace QuizApi.Entities;

public class Gamecard
{
    [BsonElement("categoryId")]
    public int CategoryId { get; set; }
    
    [BsonElement("valueId")]
    public int ValueId { get; set; }

    [BsonElement("gameMode")]
    public string GameMode { get; set; } = "";
    
    [BsonElement("isMultipleChoice")]
    public bool IsMultipleChoice { get; set; } = false;

    [BsonElement("questionImages")]
    public List<CardImage>? QuestionImages { get; set; }

    [BsonElement("eyecatcher")]
    public string? EyecatcherTitle { get; set; }

    [BsonElement("question")]
    public string QuestionText { get; set; } = "";
    
    /// <summary>
    /// Clues when the gamemode calls for it
    /// </summary>
    [BsonElement("clues")]
    public List<string>? Clues { get; set; }
    
    /// <summary>
    /// optional clues to help the players, when they are stuck
    /// </summary>
    [BsonElement("optionalClue")]
    public string? OptionalClue { get; set; }
    
    /// <summary>
    /// choices for the players
    /// </summary>
    [BsonElement("possibleAnswers")]
    public PossibleAnswers? PossibleAnswers { get; set; }
}

public class PossibleAnswers
{
    [BsonElement("correctAnswers")]
    public List<Answer>? CorrectAnswers { get; set; }
    
    [BsonElement("wrongAnswers")]
    public List<Answer>? WrongAnswers { get; set; }

    /// <summary>
    /// Should the answers be clicked or conveyed orally?
    /// </summary>
    [BsonElement("clickable")]
    public bool AreClickable { get; set; }
}

public class Answer
{
    [BsonElement("position")]
    public int? PositionNumber { get; set; }

    [BsonElement("textAnswer")]
    public string TextAnswer { get; set; } = "";
    
    [BsonElement("answerImageUrl")]
    public string? ImageUrl{ get; set; }
    
    [BsonElement("answerSoundUrl")]
    public string? SoundUrl { get; set; } 
    
    [BsonElement("explanation")]
    public string? Explanation { get; set; }
}

public class CardImage
{
    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; } = "";
    
    [BsonElement("position")]
    public int? PositionNumber { get; set; }
}