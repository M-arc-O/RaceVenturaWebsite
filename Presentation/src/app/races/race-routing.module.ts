import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RaceDetailsComponent } from './components/raceDetails/race-details.component';
import { RaceResultWrapperComponent } from './components/raceResultWrapper/race-result-wrapper.component';
import { RaceAddComponent } from './components/raceAdd/race-add.component';

const racesRoutes: Routes = [
    { path: 'addRace', component: RaceAddComponent },
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
