import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NotFoundComponent } from './NotFound';
import { HomeComponent } from './Home';
import { RaceOverviewComponent, RaceService } from './Race';

@NgModule({
  declarations: [
    AppComponent,
    NotFoundComponent,
    HomeComponent,
    RaceOverviewComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    AppRoutingModule
  ],
  providers: [RaceService],
  bootstrap: [AppComponent]
})
export class AppModule { }
