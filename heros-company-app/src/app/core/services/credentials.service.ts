import { Injectable } from '@angular/core';
import { CoreModule } from '../core.module';
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import { registerLocaleData } from '@angular/common';
import { TrainerDTO } from 'src/app/models/TrainerDTO.model';
import {map} from 'rxjs/operators'
@Injectable({
  providedIn: CoreModule
})
export class CredentialsService {
    private credentialsBaseUrl : string = environment.apiBaseUrl + "/credentials";
  constructor(private http : HttpClient) { }

  Register(userName : string, password : string) : void{

      const headers = {
          'Content-Type': 'application/json',
        }
      const url = this.credentialsBaseUrl + "/register";

        this.http.post<TrainerDTO>(url,
        {
            TrainerUserName : userName,
            Password : password
        },
        {
           headers,
           observe:'response'
        }).subscribe(
            resp =>{
                console.log(resp);
                console.log(resp.headers.get('Authorization'));
                console.log(resp.body?.trainerId);
                console.log(resp.body?.trainerUserName);
            }
        )
        
  }

  LogIn(userName : string, password : string){
      const url = this.credentialsBaseUrl + "/login";
  }

}
