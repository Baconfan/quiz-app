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
    
    [BsonElement("eyecatcher")]
    public string? EyecatcherTitle { get; set; }

    [BsonElement("question")]
    public string QuestionText { get; set; } = "";
    
    /// <summary>
    /// Clues when the gamemode calls for it
    /// </summary>
    [BsonElement("clues")]
    public List<Clue>? Clues { get; set; }
    
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
    public bool AreClickable { get; set; } = false;
}

public class Answer
{
    [BsonElement("textAnswer")]
    public string TextAnswer { get; set; } = "";
    
    [BsonElement("answerImageLink")]
    public string? ImageLink { get; set; }
    
    [BsonElement("answerSoundLink")]
    public string? SoundLink { get; set; } 
    
    [BsonElement("explanation")]
    public string? Explanation { get; set; }
}

public class Clue
{
    [BsonElement("clueText")]
    public string ClueText { get; set; } = "";
    
    [BsonElement("clueImageLink")]
    public string? ClueImageLink { get; set; } = "";
}