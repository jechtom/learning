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
}

export class Option {
  content: string;
  id: number;
}
