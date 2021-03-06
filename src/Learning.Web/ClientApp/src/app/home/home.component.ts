import { Component } from '@angular/core';
import { StateService } from '../state.service';
import { Question, QuestionGroup, Option } from '../dtos';

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
  public IsSending: boolean = false;

  selectQuestion(q: Question, index: number) {
    this.Question = q;
    this.QuestionIndex = index;
  }

  toggleAnswer(q: Option) {
    q.isSelected = !q.isSelected;
  }

  submitQuestion(q: Question) {
    if (this.IsSending) return;
    this.IsSending = true;
    this.state.submitQuestion(q.id, q.options)
      .then(r => {
        this.Question = r; // refresh
        this.IsSending = false;
      }, e => {
        this.IsSending = false;
      })
  }
}
