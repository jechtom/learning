export class StatusRefreshDto {
  groups: QuestionGroup[];
}

export class QuestionGroup {
  name: string;
  questions: Question[];
}

export class Question {
  content: string;
  id: number;
  options: Option[];
  canAnswer: boolean;
  isAnswered: boolean;
  isCorrect: boolean;
}

export class Option {
  content: string;
  id: number;
  isSelected: boolean;
  isCorrect: boolean;
}
