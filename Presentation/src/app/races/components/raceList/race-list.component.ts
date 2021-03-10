import { AfterViewInit, Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { select, Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { ComponentBase, UserService } from "src/app/shared";
import { IBase } from "src/app/store";
import { RaceViewModel } from "../../shared/models";
import { deleteRaceSelector, IRacesState, loadRacesSelector, racesSelector } from "../../store";
import * as racesActions from '../../store/actions/race.actions';

@Component({
    selector: 'app-race-list',
    templateUrl: './race-list.component.html',
})
export class RaceListComponent extends ComponentBase implements OnInit {
    public races$: Observable<RaceViewModel[]>;
    public loadRacesBase$: Observable<IBase>;
    public deleteRaceBase$: Observable<IBase>;

    public selectedRace: RaceViewModel;

    constructor(
        private store: Store<IRacesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.races$ = this.store.pipe(select(racesSelector));
        this.loadRacesBase$ = this.store.pipe(select(loadRacesSelector));
        this.deleteRaceBase$ = this.store.pipe(select(deleteRaceSelector));
        
        this.userService.loggedIn$.subscribe(loggedin => {
            if (loggedin) {
                this.store.dispatch(new racesActions.LoadRacesAction());
            }
        });
    }

    ngOnInit(): void {
        this.resetSelectedRace();

        this.loadRacesBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.deleteRaceBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetSelectedRace();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    private resetSelectedRace(): void {
        this.selectedRace = new RaceViewModel();
        this.selectedRace.raceId = undefined;
    }

    detailsClicked(race: RaceViewModel): void {
        this.selectedRace = race;
        this.router.navigate(['raceDetails', race.raceId])
    }
}