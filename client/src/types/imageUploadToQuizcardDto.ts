export type ImageUploadMetaData = {
  quizcardId: QuizcardId;
  imageAssignment: ImageAssignment;
  arrayPosition: number | null;
}

export type QuizcardId = {
  quizboardId: string;
  categoryId: number;
  valueId: number;
}

export enum ImageAssignment {
  NoAssigment = 0,
  QuestionImage = 1,
  CorrectAnswer = 2,
  WrongAnswer = 3
}
