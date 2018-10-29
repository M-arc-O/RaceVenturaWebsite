import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RaceService } from './shared/race.service';
import { RaceOverviewComponent } from './components/raceOverview/race-overview.component';
import { RacesRoutingModule } from './race-routing.module';

@NgModule({
  declarations: [
    RaceOverviewComponent,
  ],
  imports: [
    CommonModule,
    RacesRoutingModule
  ],
  providers: [RaceService],
})
export class RacesModule { }
