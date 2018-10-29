import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotFoundComponent } from './NotFound';
import { HomeComponent } from './Home';
import { RaceOverviewComponent } from './Race';

const appRoutes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'races', component: RaceOverviewComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '404', component: NotFoundComponent },
  { path: '**', redirectTo: '404' },
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
