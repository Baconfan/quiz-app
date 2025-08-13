export type QuizBoard = {
  id: string;
  quizboardTitle: string;
  quizboardDescription: string;
  editorIds: number[];
  categories: string[];
  valuesAscending: number[];
  gamecards: GamecardType[];
}

export type GamecardType = {
  quizboardId: string;

  // categoryId = position in the categories array, categoryText = topic of the column
  categoryId: number | null;
  // categoryText: string;

  // valueId = position in the values array, valueText = points given to card
  valueId: number | null;
  // valueText: string;

  gameMode: string;

  isMultipleChoice: boolean;

  eyecatcherTitle: string;
  questionText: string;
  questionImages: CardImage[];

  // Answer
  optionalClue: string;

  clues: string[];

  possibleAnswers: PossibleAnswers;
}

export type PossibleAnswers = {
  correctAnswers: AnswerType[],
  wrongAnswers: AnswerType[],
  areClickable: boolean
}

export type AnswerType = {
  textAnswer: string;
  imageUrl: string;
  soundUrl: string;
  explanation: string;
}

export type CardImage = {
  imageUrl: string;
  positionNumber: number | null;
}

