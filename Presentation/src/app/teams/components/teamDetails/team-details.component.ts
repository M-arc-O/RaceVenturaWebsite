import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { TeamDetailViewModel, TeamRequest, TeamViewModel } from '../../shared';
import { ITeamsState, loadSelectedTeamSelector, selectedTeamSelector } from '../../store';
import * as teamActions from '../../store/actions/team.actions';

@Component({
    selector: 'app-team-details',
    templateUrl: './team-details.component.html'
})
export class TeamDetailsComponent extends ComponentBase implements OnInit, OnChanges {
    @Input() selectedTeam: TeamViewModel;

    public teamDetails$: Observable<TeamDetailViewModel>;
    public teamDetailsLoad$: Observable<IBase>;

    public addEditType = AddEditType;

    constructor(private store: Store<ITeamsState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.teamDetails$ = this.store.pipe(select(selectedTeamSelector));
        this.teamDetailsLoad$ = this.store.pipe(select(loadSelectedTeamSelector));
    }

    public ngOnInit(): void {
        this.teamDetailsLoad$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    public ngOnChanges(changes: SimpleChanges): void {
        this.store.dispatch(new teamActions.LoadTeamDetailsAction(this.getTeamRequest()));
    }

    public RemoveTeamClicked(): void {
        this.store.dispatch(new teamActions.DeleteTeamAction(this.selectedTeam));
    }

    private getTeamRequest(): TeamRequest {
        const request = new TeamRequest();
        request.teamId = this.selectedTeam.id;
        request.raceId = this.selectedTeam.raceId;

        return request;
    }
}
