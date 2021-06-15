import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
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
        private formBuilder: FormBuilder,
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
                Validators.pattern(new RegExp('(?=.*\\d)(?=.*[A-Z])(?=.*\\W)')),
                Validators.minLength(8),
            ])
        })
    }
    async Submit() {
        
        const urlSnapshot = this.route.snapshot.url;

        if (urlSnapshot[urlSnapshot.length - 1].path === 'sign-up') {
            let response = await this.credentials.Register(this.credentialsForm.value.username, this.credentialsForm.value.password);
            if (response instanceof HttpErrorResponse) {
                alert(response.error)
            }
            else {
                if (response.authorization !== null && response.trainerId !== undefined && response.trainerUserName !== undefined) {
                    localStorage.setItem('token', response.authorization);
                    this.router.navigate(['heros']);
                }
            }
        }
        if (urlSnapshot[urlSnapshot.length - 1].path === 'sign-in') {
            let response = await this.credentials.LogIn(this.credentialsForm.value.username, this.credentialsForm.value.password);
            if (response instanceof HttpErrorResponse) {
                alert(response.error)
            }
            else {
                if (response.authorization !== null && response.trainerId !== undefined && response.trainerUserName !== undefined) {
                    localStorage.setItem('token', response.authorization);
                    this.router.navigate(['heros']);
                }
            }

        }
    }
}
