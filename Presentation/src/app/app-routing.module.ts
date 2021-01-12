import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppSupportComponent } from './components/appsupport/app.support.component';
import { ContactComponent } from './components/contact/contact.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './components/notFound/not-found.component';

const appRoutes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'appsupport', component: AppSupportComponent },
  { path: 'contact', component: ContactComponent },
  { path: '404', component: NotFoundComponent },
  { path: '**', redirectTo: '404' },
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes, { useHash: true, relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
