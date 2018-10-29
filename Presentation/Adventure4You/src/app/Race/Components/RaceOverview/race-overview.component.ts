import { Component, OnInit } from '@angular/core';
import { RaceViewModel } from '../../Shared/Models/race-view-model';
import { RaceService } from '../../Shared/race.service';

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
