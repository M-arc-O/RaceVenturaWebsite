import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { ValidationModule } from '../shared/components/validation/validation.module';
import { StageAddComponent } from '../stages/components/stageAdd/stage-add.component';
import { StageDetailsComponent } from '../stages/components/stageDetails/stage-details.component';
import { StageOverviewComponent } from '../stages/components/stageOverview/stage-overview.component';
import { StageService } from '../stages/shared';
import { StagesEffects } from '../stages/store/effects/stages.effects';
import { RaceAddComponent } from './components/raceAdd/race-add.component';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';
import { RaceOverviewComponent } from './components/raceOverview/race-overview.component';
import { RacesRoutingModule } from './race-routing.module';
import { RaceService } from './shared/race.service';
import { RacesEffects } from './store/effects/races.effects';
import { racesReducers } from './store/reducers';
import { stagesReducers } from '../stages/store/reducers';

@NgModule({
  declarations: [
    RaceOverviewComponent,
    RaceDetailsComponent,
    RaceAddComponent,
    StageOverviewComponent,
    StageDetailsComponent,
    StageAddComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RacesRoutingModule,
    StoreModule.forFeature('racesFeature', racesReducers),
    StoreModule.forFeature('stagesFeature', stagesReducers),
    EffectsModule.forFeature([RacesEffects, StagesEffects]),
    ValidationModule
  ],
  providers: [RaceService, StageService],
})
export class RacesModule { }
