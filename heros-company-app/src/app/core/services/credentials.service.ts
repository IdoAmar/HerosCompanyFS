import { Injectable } from '@angular/core';
import { CoreModule } from '../core.module';
import { HttpClient, HttpErrorResponse } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import { TrainerDTO } from 'src/app/models/TrainerDTO.model';
import { map } from 'rxjs/operators'
import { LogInResponse } from 'src/app/models/LogInResponse.model';
@Injectable({
    providedIn: CoreModule
})
export class CredentialsService {
    private credentialsBaseUrl: string = environment.apiBaseUrl + "/credentials";

    constructor(private http: HttpClient) { }

    //register request
    Register(userName: string, password: string): Promise<LogInResponse | HttpErrorResponse> {

        const headers = {
            'Content-Type': 'application/json',
        }
        const url = this.credentialsBaseUrl + "/register";

        return this.http.post<TrainerDTO>(url,
            {
                TrainerUserName: userName,
                Password: password
            },
            {
                headers,
                //add header to response
                observe: 'response'
            }).pipe(
                //map response to object
                map((r) => {
                    const logInResponse: LogInResponse = {
                        authorization: r.headers.get('Authorization'),
                        trainerId: r.body?.trainerId,
                        trainerUserName: r.body?.trainerUserName
                    }
                    return logInResponse;
                }),
            ).toPromise().catch((e: HttpErrorResponse) => e);

    }

    //loging request
    LogIn(userName: string, password: string) {
        const headers = {
            'Content-Type': 'application/json',
        }
        const url = this.credentialsBaseUrl + "/login";

        return this.http.post<TrainerDTO>(url,
            {
                TrainerUserName: userName,
                Password: password
            },
            {
                headers,
                //add header to response
                observe: 'response'
            }).pipe(
                //map response to object
                map((r) => {
                    const logInResponse: LogInResponse = {
                        authorization: r.headers.get('Authorization'),
                        trainerId: r.body?.trainerId,
                        trainerUserName: r.body?.trainerUserName
                    }
                    return logInResponse;
                }),
            ).toPromise().catch((e: HttpErrorResponse) => {
                return e;
            });
    }
}
