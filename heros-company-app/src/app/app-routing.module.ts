import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HerosPageComponent } from './heros/pages/heros-page/heros-page.component';
import { CredentialsPageComponent } from './home/pages/credentials-page/credentials-page.component';
import { HomePageComponent } from './home/pages/home-page/home-page.component';

const routes: Routes = [
    { path:'home', component: HomePageComponent},
    { path:'heros',component: HerosPageComponent},
    { path:'home/sign-in',component: CredentialsPageComponent},
    { path:'home/sign-up',component: CredentialsPageComponent},
    { path: '' , redirectTo:'home' , pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
