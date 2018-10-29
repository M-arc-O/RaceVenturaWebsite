import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RaceViewModel, RaceService } from '../../shared/';

@Component({
    selector: 'app-race-overview',
    templateUrl: './race-overview.component.html',
})
export class RaceOverviewComponent implements OnInit {
    races: RaceViewModel[];

    constructor(private service: RaceService,
        private router: Router) {
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
}
