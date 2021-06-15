import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { HerosDataService } from 'src/app/core/services/heros-data.service';
import { HeroDTO } from 'src/app/models/HeroDTO.model';

@Component({
    selector: 'app-heros-page',
    templateUrl: './heros-page.component.html',
    styleUrls: ['./heros-page.component.css']
})
export class HerosPageComponent implements OnInit {
    public herosList!: Promise<HeroDTO[]>;
    constructor(private dataService: HerosDataService) { }

    async ngOnInit(): Promise<void> {
        let token = localStorage.getItem('token');
        if (token) {
            this.herosList = this.dataService.GetHeros(token);
        }
    }

}
