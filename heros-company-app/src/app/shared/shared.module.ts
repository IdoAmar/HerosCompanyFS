import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardComponent } from './components/card/card.component';
import { ScrollViewComponent } from './components/scroll-view/scroll-view.component';



@NgModule({
  declarations: [
    CardComponent,
    ScrollViewComponent,
  ],
  imports: [
    CommonModule,
  ],
  exports: [
      CardComponent,
      ScrollViewComponent
  ]
})
export class SharedModule { }
