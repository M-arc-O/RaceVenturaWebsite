import { Component, OnInit, Input } from '@angular/core';
import { ComponentBase, UserService } from 'src/app/shared';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { TeamResultViewModel } from '../../shared/models';
import * as raceActions from '../../store/actions/race.actions';
import { resultStateSelector } from '../../store';
import { Observable } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CarouselService } from 'src/app/components/carousel/carousel.service';

@Component({
    selector: 'app-race-result',
    templateUrl: './race-result.component.html'
})
export class RaceResultComponent extends ComponentBase implements OnInit {
    @Input() raceId: string;

    public raceResult$: Observable<TeamResultViewModel[]>;

    constructor(private store: Store<TeamResultViewModel[]>,
        private modalService: NgbModal,
        userService: UserService,
        carouselService: CarouselService,
        router: Router) {
        super(userService, carouselService, router);
        this.raceResult$ = this.store.pipe(select(resultStateSelector));
    }

    public ngOnInit(): void {
        this.store.dispatch(new raceActions.GetRaceResultAction(this.raceId));
    }

    public refresh(): void {
        this.store.dispatch(new raceActions.GetRaceResultAction(this.raceId));
    }

    public openModal(content: any): void {
        this.modalService.open(content, { size: 'xl', scrollable: true });
    }

    public GetRaceDurationString(value: string): string {
        if (value !== null && value !== undefined) {
            let daysAndRest = value.split(`.`);
            let timeArray = new Array(3);
            let days = '00';

            switch (daysAndRest.length) {
                case 3:
                    days = daysAndRest[0];
                    timeArray = daysAndRest[1].split(':');
                    break; 
                default:
                    timeArray = daysAndRest[0].split(':');
                break;
            }

            return `${days}-${timeArray[0]}-${timeArray[1]}-${timeArray[2]}`;
        }

        return "";
    }
}
