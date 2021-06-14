import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HerosPageComponent } from './pages/heros-page/heros-page.component';
import { SharedModule } from '../shared/shared.module';
import {  HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [
    HerosPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    HttpClientModule
  ],
  exports:[
      HerosPageComponent
  ]
})
export class HerosModule { }
