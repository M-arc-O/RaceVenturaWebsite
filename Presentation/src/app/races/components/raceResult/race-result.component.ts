import { Component, OnInit, Input } from '@angular/core';
import { ComponentBase, UserService } from 'src/app/shared';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { TeamResultViewModel } from '../../shared/models';
import * as raceActions from '../../store/actions/race.actions';
import { resultStateSelector } from '../../store';
import { Observable } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

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
        router: Router) {
        super(userService, router);
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
            let values = value.split(`.`);
            let days = '00';
            if (values.length > 1) {
                days = values[0]
                values = values[1].split(`:`);
            }
            else {
                values = value.split(`:`);
            }
    
            return `${days}-${values[0]}-${values[1]}-${values[2]}`; 
        }

        return "";
    }
}
