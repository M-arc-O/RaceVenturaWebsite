import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { AddEditType } from '../../../shared';
import { RaceStoreModel } from '../../shared/models';
import { IRacesState, loadSelectedRaceSelector, selectedRaceSelector } from '../../store';
import * as raceActions from '../../store/actions/race.actions';
import { RaceComponentBase } from '../race-component-base.component';

@Component({
    selector: 'app-race-details',
    templateUrl: './race-details.component.html'
})
export class RaceDetailsComponent extends RaceComponentBase implements OnInit, OnChanges {
    @Input() raceId: string;

    public raceDetails$: Observable<RaceStoreModel>;
    public raceDetailsLoad$: Observable<IBase>;

    public addEditType = AddEditType;

    constructor(private store: Store<IRacesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.raceDetails$ = this.store.pipe(select(selectedRaceSelector));
        this.raceDetailsLoad$ = this.store.pipe(select(loadSelectedRaceSelector));
    }

    public ngOnInit(): void {
        this.raceDetailsLoad$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    public ngOnChanges(changes: SimpleChanges): void {
        this.store.dispatch(new raceActions.LoadRaceDetailsAction(this.raceId));
    }

    public RemoveRaceClicked(): void {
        this.store.dispatch(new raceActions.DeleteRaceAction(this.raceId));
    }
}
