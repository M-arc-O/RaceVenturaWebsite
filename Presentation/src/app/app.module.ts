import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment'; // Angular CLI environemnt
import { AccountModule } from './account/account.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuComponent } from './components/menu/menu.component';
import { NotFoundComponent } from './components/notFound/not-found.component';
import { RacesModule } from './races/races.module';
import { metaReducers, reducers } from './reducers';
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
    AppRoutingModule,
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: environment.production, // Restrict extension to log-only mode
    }),
    StoreModule.forRoot(reducers, { metaReducers })
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
