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
import { RaceAddComponent } from './components/raceAdd/race-add.component';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';
import { RacesOverviewComponent } from './components/racesOverview/races-overview.component';
import { StageAddComponent } from './components/stageAdd/stage-add.component';
import { StageDetailsComponent } from './components/stageDetails/stage-details.component';
import { StagesOverviewComponent } from './components/stagesOverview/stages-overview.component';
import { TeamAddComponent } from './components/teamAdd/team-add.component';
import { TeamDetailsComponent } from './components/teamDetails/team-details.component';
import { TeamsOverviewComponent } from './components/teamsOverview/teams-overview.component';
import { RacesRoutingModule } from './race-routing.module';
import { RacesService, StagesService, TeamsService } from './shared';
import { RaceEffects, StageEffects, TeamEffects } from './store/effects';
import { racesReducers } from './store/reducers/races';
import { stagesReducers } from './store/reducers/stages';
import { teamsReducers } from './store/reducers/teams';

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
    PointAddComponent,
    TeamsOverviewComponent,
    TeamDetailsComponent,
    TeamAddComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RacesRoutingModule,
    StoreModule.forFeature('racesFeature', racesReducers),
    StoreModule.forFeature('stagesFeature', stagesReducers),
    StoreModule.forFeature('pointsFeature', pointsReducers),
    StoreModule.forFeature('teamsFeature', teamsReducers),
    EffectsModule.forFeature([RaceEffects, StageEffects, PointsEffects, TeamEffects]),
    ValidationModule,
    PipesModule
  ],
  providers: [RacesService, StagesService, PointsService, TeamsService],
})
export class RacesModule { }
