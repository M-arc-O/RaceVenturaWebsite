import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { PointAddComponent } from '../points/components/pointAdd/point-add.component';
import { PointDetailsComponent } from '../points/components/pointDetails/point-details.component';
import { PointsOverviewComponent } from '../points/components/pointsOverview/points-overview.component';
import { PointsService } from '../points/shared';
import { PointsEffects } from '../points/store/effects/points.effects';
import { pointsReducers } from '../points/store/reducers/points.reducers';
import { ValidationModule } from '../shared/components/validation/validation.module';
import { PipesModule } from '../shared/pipes/pipes.module';
import { StageAddComponent } from '../stages/components/stageAdd/stage-add.component';
import { StageDetailsComponent } from '../stages/components/stageDetails/stage-details.component';
import { StagesOverviewComponent } from '../stages/components/stagesOverview/stages-overview.component';
import { StagesService } from '../stages/shared';
import { StagesEffects } from '../stages/store/effects/stages.effects';
import { stagesReducers } from '../stages/store/reducers';
import { RaceAddComponent } from './components/raceAdd/race-add.component';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';
import { RacesOverviewComponent } from './components/racesOverview/races-overview.component';
import { RacesRoutingModule } from './race-routing.module';
import { RacesService } from './shared';
import { RacesEffects } from './store/effects/races.effects';
import { racesReducers } from './store/reducers';

@NgModule({
  declarations: [
    RacesOverviewComponent,
    RaceDetailsComponent,
    RaceAddComponent,
    StagesOverviewComponent,
    StageDetailsComponent,
    StageAddComponent,
    PointsOverviewComponent,
    PointDetailsComponent,
    PointAddComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RacesRoutingModule,
    StoreModule.forFeature('racesFeature', racesReducers),
    StoreModule.forFeature('stagesFeature', stagesReducers),
    StoreModule.forFeature('pointsFeature', pointsReducers),
    EffectsModule.forFeature([RacesEffects, StagesEffects, PointsEffects]),
    ValidationModule,
    PipesModule
  ],
  providers: [RacesService, StagesService, PointsService],
})
export class RacesModule { }
