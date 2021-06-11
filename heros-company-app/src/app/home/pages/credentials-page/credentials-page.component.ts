import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

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
        private router: Router
    ) { }

    ngOnInit(): void {
        this.currentPage = this.route.snapshot.url[1].path;
        this.credentialsForm = new FormGroup({
            "userName": new FormControl("", [
                Validators.required,
            ]),
            "password": new FormControl("", [
                Validators.required,
                Validators.pattern(new RegExp('(?=.*\\d)(?=.*[A-Z])(?=.*\\W)')),
                Validators.minLength(8),
            ])
        })
    }
    Submit(){
        this.router.navigate(['/heros']);
    }

}
