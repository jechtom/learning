import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StatusRefreshDto, QuestionGroup, Question, Option } from './dtos';
import { forEach } from '@angular/router/src/utils/collection';

@Injectable()
export class StateService {

  constructor(private http: HttpClient) {
    this.token = localStorage.getItem("learning-token");
    this.name = localStorage.getItem("learning-name");

    if (this.token != null && this.token != undefined && this.token != "") {
      this.refreshStatus();
    }
  }

  public name: string;
  public token: string;
  public questionGroups: QuestionGroup[] = [];
  public questionGroup: QuestionGroup = null;
  
  public updateProfile() {
    localStorage.setItem("learning-token", this.token);
    localStorage.setItem("learning-name", this.name);
    this.http.post("/user/profile", {
      Token: this.token,
      Name: this.name
    }).toPromise().then(l => {
      console.log("Profile updated")
      this.refreshStatus();
    });
  }

  public refreshStatus(): Promise<StatusRefreshDto> {
    var result = this.http.post<StatusRefreshDto>("/user/status", { Token: this.token })
      .toPromise();

    result.then(l => {
          this.questionGroups = l.groups;
    });

    return result;
  }

  public submitQuestion(id: number, answers: Option[]) : Promise<Question> {
    return new Promise<Question>((resolve, reject) => {
      this.http.post<Question>("/user/submit", { Token: this.token, Id: id, Answers: answers })
        .toPromise()
        .then(r => {
          this.refreshStatus().then(rs => {
            // find question in refreshed response
            var question = this.findQuestion(rs.groups, id);
            if (question == null) {
              reject("Can't find question with given Id");
            }
            else {
              resolve(question);
            }
          }, rse => {
            // failed to fetch refreshed response
            reject(rse);
          });
        }, e => {
          // failed to submit answer
          reject(e);
        });
    });
  }

  private findQuestion(questionGroups: QuestionGroup[], questionId: number): Question {
    for (var i = 0; i < questionGroups.length; i++) {
      var f = questionGroups[i].questions.filter(q => q.id == questionId);
      if (f.length > 0) {
        return f[0];
      }
    }
    return null;
  }
}
