import { Component, Input, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { TeamViewModel } from '../../shared';
import { deleteTeamSelector, ITeamsState, loadTeamsSelector, teamsListSelector } from '../../store';
import * as teamsActions from '../../store/actions/team.actions';

@Component({
    selector: 'app-teams-overview',
    templateUrl: './teams-overview.component.html',
    styleUrls: ['./teams-overview.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class TeamsOverviewComponent extends ComponentBase implements OnInit {
    @Input() raceId: string;

    public teams$: Observable<TeamViewModel[]>;
    public loadTeamsBase$: Observable<IBase>;
    public deleteTeamBase$: Observable<IBase>;
    public selectedTeam: TeamViewModel;
    public addEditType = AddEditType;

    constructor(
        private store: Store<ITeamsState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.teams$ = this.store.pipe(select(teamsListSelector));
        this.loadTeamsBase$ = this.store.pipe(select(loadTeamsSelector));
        this.deleteTeamBase$ = this.store.pipe(select(deleteTeamSelector));
    }

    ngOnInit(): void {
        this.loadTeamsBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.deleteTeamBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.selectedTeam.id = undefined;
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.selectedTeam = new TeamViewModel();
        this.selectedTeam.raceId = this.raceId;

        this.getTeams();
    }

    getTeams(): void {
        this.store.dispatch(new teamsActions.LoadTeamsAction(this.raceId));
    }

    detailsClicked(team: TeamViewModel): void {
        this.selectedTeam = team;
    }
}
