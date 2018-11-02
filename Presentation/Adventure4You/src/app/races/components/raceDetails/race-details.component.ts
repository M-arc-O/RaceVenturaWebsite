import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-race-details',
    templateUrl: './race-details.component.html'
})
export class RaceDetailsComponent {
    @Input() raceId: number;

    constructor() {

    }
}
