import { Component } from '@angular/core';
import { StateService } from '../state.service';
import { Question } from '../dtos';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(public state: StateService) { }

  refreshStatus() {
    this.Question = null;
    this.state.refreshStatus();
  }

  public Question: Question = null;
  public QuestionIndex: number;

  selectQuestion(q: Question, index: number) {
    this.Question = q;
    this.QuestionIndex = index;
  }
}
