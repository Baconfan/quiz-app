export type QuizBoard = {
  id: string;
  quizboardTitle: string;
  quizboardDescription: string;
  editorIds: number[];
  categories: string[];
  valuesAscending: number[];
  gamecards: Gamecard[];
}

export type Gamecard = {
  // categoryId = position in the categories array, categoryText = topic of the column
  categoryId: number;
  // categoryText: string;

  // valueId = position in the values array, valueText = points given to card
  valueId: number;
  // valueText: string;

  gameMode: string;
  eyecatcherTitle: string;
  questionText: string;

  // Answer
  correctAnswer: string;
}
