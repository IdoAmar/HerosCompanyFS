import { HttpClient } from '@angular/common/http';
import { tokenize } from '@angular/compiler/src/ml_parser/lexer';
import { Injectable } from '@angular/core';
import { HeroDTO } from 'src/app/models/HeroDTO.model';
import { environment } from 'src/environments/environment';
import { CoreModule } from '../core.module';
import { map } from 'rxjs/operators'

@Injectable({
    providedIn: CoreModule
})
export class HerosDataService {
    private HerosDataBaseUrl: string = environment.apiBaseUrl +"/heros";
    constructor(private http: HttpClient) { }
    GetHeros(token : string):Promise<HeroDTO[]>{
        const headers = {
            'Content-Type': 'application/json',
            'Authorization': token
        }
        return this.http.get<HeroDTO[]>(this.HerosDataBaseUrl,
            {
                headers,
            }).pipe(
                map(l => l.sort((h1, h2)=> h2.currentPower -h1.currentPower ))
            ).toPromise()
    }
    TrainHero(heroID : string, token : string): Promise<HeroDTO>{
        const headers = {
            'Authorization': token,
        }
        
        const url = this.HerosDataBaseUrl + '/train' + '/' + heroID;
        return this.http.patch<HeroDTO>(url,{},
            {
                headers,
            }).toPromise();
    }
}
