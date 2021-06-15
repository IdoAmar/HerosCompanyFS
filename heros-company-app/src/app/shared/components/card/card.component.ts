import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HerosDataService } from 'src/app/core/services/heros-data.service';
import { HeroDTO } from 'src/app/models/HeroDTO.model';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {
    @Input() heroDTO! : HeroDTO ;
  constructor(
      private herosService : HerosDataService,
      private router: Router
      ) { }

  ngOnInit(): void {
  }

  async onHeroTrained(){
      let token = localStorage.getItem('token');
      if(token != null)
      {
          let newData = await this.herosService.TrainHero(this.heroDTO.heroId,token)
          
          this.heroDTO.currentPower = newData.currentPower;
          this.heroDTO.trainable = newData.trainable;
      }
      else{
          alert("please relog");
          this.router.navigate(['']);
      }
  }
}
