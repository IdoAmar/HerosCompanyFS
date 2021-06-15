import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CredentialsService } from 'src/app/core/services/credentials.service';

@Component({
    selector: 'app-credentials-page',
    templateUrl: './credentials-page.component.html',
    styleUrls: ['./credentials-page.component.css']
})
export class CredentialsPageComponent implements OnInit {

    currentPage!: string;
    credentialsForm!: FormGroup;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private credentials: CredentialsService
    ) { }

    ngOnInit(): void {
        this.currentPage = this.route.snapshot.url[1].path;
        this.credentialsForm = new FormGroup({
            "username": new FormControl("", [
                Validators.required,
            ]),
            "password": new FormControl("", [
                Validators.required,
                //validating password by regex pattern "1 digit 1 capital letter 1 special letter"
                Validators.pattern(new RegExp('(?=.*\\d)(?=.*[A-Z])(?=.*\\W)')),
                Validators.minLength(8),
            ])
        })
    }

    async Submit() {

        const urlSnapshot = this.route.snapshot.url;
        //checking if inside sign-up page
        if (urlSnapshot[urlSnapshot.length - 1].path === 'sign-up') {
            let response = await this.credentials.Register(this.credentialsForm.value.username, this.credentialsForm.value.password);
            //show error to user
            if (response instanceof HttpErrorResponse) {
                alert(response.error.detail);
            }
            else {
                if (response.authorization !== null && response.trainerId !== undefined && response.trainerUserName !== undefined) {
                    //store recieved token in local storage and navigate to requested route
                    localStorage.setItem('token', response.authorization);
                    this.router.navigate(['heros']);
                }
            }
        }

        //checking if inside sign-in page
        if (urlSnapshot[urlSnapshot.length - 1].path === 'sign-in') {
            let response = await this.credentials.LogIn(this.credentialsForm.value.username, this.credentialsForm.value.password);
            //show error to user
            if (response instanceof HttpErrorResponse) {
                console.log(response);
                alert(response.error.detail);
            }
            else {
                if (response.authorization !== null && response.trainerId !== undefined && response.trainerUserName !== undefined) {
                    //store recieved token in local storage and navigate to requested route
                    localStorage.setItem('token', response.authorization);
                    this.router.navigate(['heros']);
                }
            }

        }
    }
}
