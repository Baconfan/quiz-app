namespace QuizApi.Models;

public class GamecardDto
{
    public required string QuizboardId { get; set; }
    
    public required int CategoryId { get; set; }
    
    public required int ValueId { get; set; }
    
    public string GameMode { get; set; } = "";
    
    /// <summary>
    /// Should multiple choices be presented?
    /// </summary>
    public bool IsMultipleChoice { get; set; }
    
    /// <summary>
    /// images used with the questionText
    /// </summary>
    public List<CardImage> QuestionImages { get; set; } = [];

    public string EyecatcherTitle { get; set; } = "";
    
    public string QuestionText { get; set; } = "";
    
    /// <summary>
    /// Clues when the gamemode calls for it
    /// </summary>
    public List<string> Clues { get; set; } = [];

    /// <summary>
    /// optional clues to help the players, when they are stuck
    /// </summary>
    public string OptionalClue { get; set; } = "";

    /// <summary>
    /// choices for the players and also the correct answers
    /// </summary>
    public PossibleAnswers PossibleAnswers { get; set; } = new();
}

public class PossibleAnswers
{
    public List<Answer> CorrectAnswers { get; set; } = [];

    public List<Answer> WrongAnswers { get; set; } = [];

    /// <summary>
    /// Should the answers be clicked or conveyed orally?
    /// </summary>
    public bool AreClickable { get; set; }
}

public class Answer
{
    public int? PositionNumber { get; set; }
    
    public string TextAnswer { get; set; } = "";

    public string ImageUrl { get; set; } = "";

    public string SoundUrl { get; set; } = "";

    public string Explanation { get; set; } = "";
}

public class CardImage
{
    public string ImageUrl { get; set; } = "";
    
    public int? PositionNumber { get; set; }
}