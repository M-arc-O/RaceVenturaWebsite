import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RaceOverviewComponent } from './components/raceOverview/race-overview.component';

const racesRoutes: Routes = [
    { path: 'races', component: RaceOverviewComponent },
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
