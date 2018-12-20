import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AddRaceComponent } from './components/addRace/add-race.component';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';
import { RaceOverviewComponent } from './components/raceOverview/race-overview.component';
import { RacesRoutingModule } from './race-routing.module';
import { RaceService } from './shared/race.service';
import { RacesEffects } from './store/effects/races.effects';
import { racesReducers } from './store/reducers';

@NgModule({
  declarations: [
    RaceOverviewComponent,
    RaceDetailsComponent,
    AddRaceComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RacesRoutingModule,
    StoreModule.forFeature('racesFeature', racesReducers),
    EffectsModule.forFeature([RacesEffects])
  ],
  providers: [RaceService],
})
export class RacesModule { }
