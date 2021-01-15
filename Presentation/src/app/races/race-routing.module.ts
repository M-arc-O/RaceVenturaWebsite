import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RacesOverviewComponent } from './components/racesOverview/races-overview.component';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';
import { RaceResultWrapperComponent } from './components/raceResultWrapper/race-result-wrapper.component';

const racesRoutes: Routes = [
    { path: 'races', component: RacesOverviewComponent },
    { path: 'raceDetails/:id', component: RaceDetailsComponent },
    { path: "results", component: RaceResultWrapperComponent }
];

@NgModule({
    imports: [
        RouterModule.forChild(racesRoutes)
    ],
    exports: [
        RouterModule
    ]
})

export class RacesRoutingModule { }
