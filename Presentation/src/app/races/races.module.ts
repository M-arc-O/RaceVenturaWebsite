import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbDateAdapter, NgbDateParserFormatter, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { CustomDateAdapter, CustomDateParserFormatter } from '../shared';
import { ValidationModule } from '../shared/components/validation/validation.module';
import { PipesModule } from '../shared/pipes/pipes.module';
import { PointAddComponent } from './components/pointAdd/point-add.component';
import { PointsOverviewComponent } from './components/pointsOverview/points-overview.component';
import { RaceAddComponent } from './components/raceAdd/race-add.component';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';
import { RaceListComponent } from './components/raceList/race-list.component';
import { RaceResultComponent } from './components/raceResult/race-result.component';
import { RaceResultWrapperComponent } from './components/raceResultWrapper/race-result-wrapper.component';
import { StageAddComponent } from './components/stageAdd/stage-add.component';
import { StageDetailsComponent } from './components/stageDetails/stage-details.component';
import { StagesOverviewComponent } from './components/stagesOverview/stages-overview.component';
import { TeamAddComponent } from './components/teamAdd/team-add.component';
import { TeamDetailsComponent } from './components/teamDetails/team-details.component';
import { TeamPointsComponent } from './components/teamPoints/team-points.component';
import { TeamsOverviewComponent } from './components/teamsOverview/teams-overview.component';
import { RacesRoutingModule } from './race-routing.module';
import { PointsService, RacesDownloadService, RacesService, ResultsService, StagesService, TeamsService, VisitedPointsService } from './shared';
import { PointEffects, RaceEffects, StageEffects, TeamEffects } from './store/effects';
import { TeamPointVisitedEffects } from './store/effects/team-point-visited';
import { pointsReducers } from './store/reducers/points';
import { racesReducers } from './store/reducers/races';
import { stagesReducers } from './store/reducers/stages';
import { teamPointVisitedReducers } from './store/reducers/teamPointVisited';
import { teamsReducers } from './store/reducers/teams';

@NgModule({
  declarations: [
    RaceListComponent,
    RaceDetailsComponent,
    RaceAddComponent,
    RaceResultComponent,
    RaceResultWrapperComponent,
    StagesOverviewComponent,
    StageDetailsComponent,
    StageAddComponent,
    PointsOverviewComponent,
    PointAddComponent,
    TeamsOverviewComponent,
    TeamDetailsComponent,
    TeamAddComponent,
    TeamPointsComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RacesRoutingModule,
    StoreModule.forFeature('racesFeature', racesReducers),
    StoreModule.forFeature('stagesFeature', stagesReducers),
    StoreModule.forFeature('pointsFeature', pointsReducers),
    StoreModule.forFeature('teamsFeature', teamsReducers),
    StoreModule.forFeature('teamPointVisitedFeature', teamPointVisitedReducers),
    EffectsModule.forFeature([RaceEffects, StageEffects, PointEffects, TeamEffects, TeamPointVisitedEffects]),
    ValidationModule,
    PipesModule,    
    NgbModule
  ],
  exports: [
    RaceListComponent
  ],
  providers: [
    RacesService, 
    RacesDownloadService,
    StagesService, 
    PointsService, 
    TeamsService, 
    ResultsService, 
    VisitedPointsService,
    {provide: NgbDateAdapter, useClass: CustomDateAdapter},
    {provide: NgbDateParserFormatter, useClass: CustomDateParserFormatter}  ]
})
export class RacesModule { }
