import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment'; // Angular CLI environemnt
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NotFoundComponent } from './components/notFound/not-found.component';
import { AccountModule } from './account/account.module';
import { RacesModule } from './races/races.module';
import { MenuComponent } from './components/menu/menu.component';
import { UserService } from './shared';
import { racesReducer } from './races/reducers/races.reducers';
import { RacesEffects } from './races/effects/races.effects';

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
    StoreModule.forRoot({races: racesReducer}),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: environment.production, // Restrict extension to log-only mode
    }),
    EffectsModule.forRoot([RacesEffects])
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
