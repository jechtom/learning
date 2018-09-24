import { Component } from '@angular/core';
import { StateService } from '../state.service';
import { Question } from '../dtos';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private state: StateService) { }

  refreshStatus() {
    this.Question = null;
    this.state.refreshStatus();
  }

  public Question: Question = null;

  selectQuestion(q: Question) {
    this.Question = q;
  }
}
