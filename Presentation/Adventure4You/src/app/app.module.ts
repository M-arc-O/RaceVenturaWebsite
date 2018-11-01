import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NotFoundComponent } from './components/notFound/not-found.component';
import { AccountModule } from './account/account.module';
import { RacesModule } from './races/races.module';
import { MenuComponent } from './components/menu/menu.component';
import { UserService } from './shared';

@NgModule({
  declarations: [
    AppComponent,
    NotFoundComponent,
    MenuComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    AccountModule,
    RacesModule,
    AppRoutingModule
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
