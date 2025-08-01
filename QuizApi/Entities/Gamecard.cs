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
    public string? OptionalClue { get; set; }
    
    /// <summary>
    /// choices for the players
    /// </summary>
    public PossibleAnswers? PossibleAnswers { get; set; }
}

public class PossibleAnswers
{
    public List<Answer>? CorrectAnswer { get; set; }
    public List<Answer>? WrongAnswers { get; set; }

    /// <summary>
    /// Should the answers be clicked or conveyed orally?
    /// </summary>
    public bool AreClickable { get; set; } = false;
}

public class Answer
{
    public string TextAnswer { get; set; } = "";
    public string? ImageLink { get; set; }
    public string? SoundLink { get; set; } 
    public string? Explanation { get; set; }
}

public class Clue
{
    public string ClueText { get; set; } = "";
    public string? ClueImageLink { get; set; } = "";
}