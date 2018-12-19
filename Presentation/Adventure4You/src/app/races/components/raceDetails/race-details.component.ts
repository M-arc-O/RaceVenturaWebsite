import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { RaceDetailViewModel, RaceUtilities } from '../../shared';
import * as raceActions from '../../store/actions/race.actions';
import { IRacesState } from '../../store/racesState.interface';
import { loadSelectedRaceSelector, selectedRaceSelector } from '../../store/selectedRace.interface';
import { FormGroup } from '@angular/forms';

@Component({
    selector: 'app-race-details',
    templateUrl: './race-details.component.html'
})
export class RaceDetailsComponent extends ComponentBase implements OnInit, OnChanges {
    @Input() raceId: number;

    public raceDetails$: Observable<RaceDetailViewModel>;
    public raceDetailsLoad$: Observable<IBase>;

    public editRaceForm: FormGroup;

    constructor(private store: Store<IRacesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.raceDetails$ = this.store.pipe(select(selectedRaceSelector));
        this.raceDetailsLoad$ = this.store.pipe(select(loadSelectedRaceSelector));
    }

    ngOnInit(): void {
        this.raceDetails$.pipe(takeUntil(this.unsubscribe$)).subscribe(details => RaceUtilities.setupForm(details));

        this.raceDetailsLoad$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.store.dispatch(new raceActions.LoadRaceDetailsAction(this.raceId));
    }
}
