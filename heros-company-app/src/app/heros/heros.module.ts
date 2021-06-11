import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HerosPageComponent } from './pages/heros-page/heros-page.component';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    HerosPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule
  ],
  exports:[
      HerosPageComponent
  ]
})
export class HerosModule { }
