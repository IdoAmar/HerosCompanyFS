import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-access',
  templateUrl: './access.component.html',
  styleUrls: ['./access.component.css']
})
export class AccessComponent implements OnInit {

  constructor(private router : Router) { }

  ngOnInit(): void {
  }
  onSignUpClicked(){
      this.router.navigate(['/home/sign-up']);
  }
  onSignInClicked(){
      this.router.navigate(['/home/sign-in']);
  }
}
