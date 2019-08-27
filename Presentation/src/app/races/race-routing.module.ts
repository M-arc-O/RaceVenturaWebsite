import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RaceOverviewComponent } from './components/raceOverview/race-overview.component';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';

const racesRoutes: Routes = [
    { path: 'races', component: RaceOverviewComponent },
    { path: 'raceDetails/:id', component: RaceDetailsComponent }
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
