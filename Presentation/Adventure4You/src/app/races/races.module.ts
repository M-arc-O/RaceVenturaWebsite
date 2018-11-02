import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RaceService } from './shared/race.service';
import { RaceOverviewComponent } from './components/raceOverview/race-overview.component';
import { RacesRoutingModule } from './race-routing.module';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';

@NgModule({
  declarations: [
    RaceOverviewComponent,
    RaceDetailsComponent
  ],
  imports: [
    CommonModule,
    RacesRoutingModule
  ],
  providers: [RaceService],
})
export class RacesModule { }
