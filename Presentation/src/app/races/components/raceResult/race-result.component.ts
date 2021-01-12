import { Component, OnInit, Input } from '@angular/core';
import { ComponentBase, UserService } from 'src/app/shared';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { TeamResultViewModel } from '../../shared/models';
import * as raceActions from '../../store/actions/race.actions';
import { resultStateSelector } from '../../store';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-race-result',
    templateUrl: './race-result.component.html'
})
export class RaceResultComponent extends ComponentBase implements OnInit {
    @Input() raceId: string;

    public raceResult$: Observable<TeamResultViewModel[]>;

    constructor(private store: Store<TeamResultViewModel[]>,
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
}
