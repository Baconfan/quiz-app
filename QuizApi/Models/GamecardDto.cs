namespace QuizApi.Models;

public class GamecardDto
{
    public required string QuizboardId { get; set; }
    
    public int CategoryId { get; set; }
    
    public int ValueId { get; set; }
    
    public string GameMode { get; set; } = "";
    
    public string? EyecatcherTitle { get; set; }
    
    public string QuestionText { get; set; } = "";
    
    /// <summary>
    /// Clues when the gamemode calls for it
    /// </summary>
    public List<Clue>? Clues { get; set; }
    
    /// <summary>
    /// optional clues to help the players, when they are stuck
    /// </summary>
    public string? OptionalClue { get; set; }
    
    /// <summary>
    /// choices for the players and also the correct answers
    /// </summary>
    public PossibleAnswer? PossibleAnswers { get; set; }
}

public class PossibleAnswer
{
    public List<Answer>? CorrectAnswers { get; set; }
    
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
