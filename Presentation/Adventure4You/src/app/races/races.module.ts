import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RaceService } from './shared/race.service';
import { RaceOverviewComponent } from './components/raceOverview/race-overview.component';
import { RacesRoutingModule } from './race-routing.module';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AddRaceComponent } from './components/addRace/add-race.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { RacesEffects } from './effects/races.effects';
import { racesReducer, SelectedRaceReducer } from './reducers';

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
    StoreModule.forFeature('racesFeature', { races: racesReducer, selectedRace: SelectedRaceReducer }),
    EffectsModule.forFeature([RacesEffects])
  ],
  providers: [RaceService],
})
export class RacesModule { }
