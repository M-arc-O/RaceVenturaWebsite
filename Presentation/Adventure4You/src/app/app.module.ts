import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment'; // Angular CLI environemnt
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NotFoundComponent } from './components/notFound/not-found.component';
import { AccountModule } from './account/account.module';
import { RacesModule } from './races/races.module';
import { MenuComponent } from './components/menu/menu.component';
import { UserService } from './shared';
import { EffectsModule } from '@ngrx/effects';

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
    AppRoutingModule,
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: environment.production, // Restrict extension to log-only mode
    })
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
