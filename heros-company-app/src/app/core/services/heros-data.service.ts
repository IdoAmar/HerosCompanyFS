import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { tokenize } from '@angular/compiler/src/ml_parser/lexer';
import { Injectable } from '@angular/core';
import { HeroDTO } from 'src/app/models/HeroDTO.model';
import { environment } from 'src/environments/environment';
import { CoreModule } from '../core.module';
import { catchError, map } from 'rxjs/operators'
import { Router } from '@angular/router';

@Injectable({
    providedIn: CoreModule
})
export class HerosDataService {

    private HerosDataBaseUrl: string = environment.apiBaseUrl + "/heros";

    constructor(
        private http: HttpClient,
        private router: Router) { }

    //get all heros request
    GetHeros(token: string): Promise<HeroDTO[]> {

        const headers = {
            'Content-Type': 'application/json',
            'Authorization': token
        }

        return this.http.get<HeroDTO[]>(this.HerosDataBaseUrl,
            {
                headers,
            }
            ).pipe(
                catchError((e: HttpErrorResponse) => {
                    //catch error in response
                    if (e.status === 401) {
                        localStorage.removeItem('token');
                        this.router.navigate([''])
                        alert("Please relog in");
                        return [];
                    }
                    alert(e.error.detail);
                    return [];
                }),
                //sort heros by power
                map(l => l.sort((h1, h2) => h2.currentPower - h1.currentPower))
            ).toPromise();
    }

    //train heros request
    TrainHero(heroID: string, token: string): Promise<HeroDTO> {

        const headers = {
            'Authorization': token,
        }

        const url = this.HerosDataBaseUrl + '/train' + '/' + heroID;
        return this.http.patch<HeroDTO>(url, {},
            {
                headers,
            }
            ).pipe(catchError((e: HttpErrorResponse) => {
                //catch error in response
                if (e.status === 401) {
                    localStorage.removeItem('token');
                    this.router.navigate([''])
                    alert("Please relog in");
                    return [];
                }
                alert(e.error.detail);
                return [];
            })
            ).toPromise();
    }
}
