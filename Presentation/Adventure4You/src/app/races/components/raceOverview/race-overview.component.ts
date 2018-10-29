import { Component, OnInit } from '@angular/core';
import { RaceViewModel, RaceService } from '../../shared/';

@Component({
    selector: 'app-race-overview',
    templateUrl: './race-overview.component.html',
})
export class RaceOverviewComponent implements OnInit {
    races: RaceViewModel[];

    constructor(private service: RaceService) {
    }

    ngOnInit(): void {
        this.service.getRaces().subscribe(races => {
            this.races = races;
        });
    }
}
