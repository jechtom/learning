import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StateService } from '../state.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private state: StateService) { }

  ngOnInit() {
    
  }
  
  save() {
    this.state.updateProfile();
  }
}
