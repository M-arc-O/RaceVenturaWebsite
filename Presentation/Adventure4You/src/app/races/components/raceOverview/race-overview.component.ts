import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RaceViewModel, RaceService } from '../../shared/';
import { ComponentBase } from 'src/app/shared';

@Component({
    selector: 'app-race-overview',
    templateUrl: './race-overview.component.html',
})
export class RaceOverviewComponent extends ComponentBase implements OnInit {
    races: RaceViewModel[];
    selectedRace: RaceViewModel;

    constructor(private service: RaceService,
        private router: Router) {
        super();
    }

    ngOnInit(): void {
        this.service.getRaces().subscribe(races => {
            this.races = races;
        },
        error => {
            if (error.status === 401) {
                this.router.navigateByUrl('home');
            }
        });
    }

    detailsClicked(race: RaceViewModel): void {
        this.selectedRace = race;
    }
}
