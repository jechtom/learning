import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StatusRefreshDto, QuestionGroup } from './dtos';

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

  public refreshStatus() {
    this.http.post<StatusRefreshDto>("/user/status", { Token: this.token })
      .toPromise()
      .then(l => {
        this.questionGroups = l.groups;
      });
  }

}
