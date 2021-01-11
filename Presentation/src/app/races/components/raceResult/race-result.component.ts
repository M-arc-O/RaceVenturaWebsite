import { Component, OnInit, Input } from '@angular/core';
import { ComponentBase, UserService } from 'src/app/shared';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { TeamResultViewModel, StageStoreModel } from '../../shared/models';
import * as raceActions from '../../store/actions/race.actions';
import { resultStateSelector, stagesSelector } from '../../store';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-race-result',
    templateUrl: './race-result.component.html'
})
export class RaceResultComponent extends ComponentBase implements OnInit {
    @Input() raceId: string;

    public raceResult$: Observable<TeamResultViewModel[]>;
    public stages$: Observable<StageStoreModel[]>;

    constructor(private store: Store<TeamResultViewModel[]>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.raceResult$ = this.store.pipe(select(resultStateSelector));
        this.stages$ = this.store.pipe(select(stagesSelector));
    }

    public ngOnInit(): void {
        this.store.dispatch(new raceActions.GetRaceResultAction(this.raceId));
    }

    public refresh(): void {
        this.store.dispatch(new raceActions.GetRaceResultAction(this.raceId));
    }
}
