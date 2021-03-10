import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { TeamStoreModel } from '../../shared/models';
import { deleteTeamSelector, ISelectedRace, teamsSelector } from '../../store';

@Component({
    selector: 'app-teams-overview',
    templateUrl: './teams-overview.component.html',
    styleUrls: ['./teams-overview.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class TeamsOverviewComponent extends ComponentBase implements OnInit {
    @Input() raceId: string;

    public teams$: Observable<TeamStoreModel[]>;
    public deleteTeamBase$: Observable<IBase>;
    public selectedTeamId: string;
    public addEditType = AddEditType;

    constructor(
        private store: Store<ISelectedRace>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.teams$ = this.store.pipe(select(teamsSelector));
        this.deleteTeamBase$ = this.store.pipe(select(deleteTeamSelector));
    }

    ngOnInit(): void {
        this.resetSelectedTeam();

        this.deleteTeamBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetSelectedTeam();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    public getSelectedTeam(teams: TeamStoreModel[]): TeamStoreModel {
        return teams.find(team => team.teamId === this.selectedTeamId);
    }

    private resetSelectedTeam() {
        this.selectedTeamId = undefined;
    }

    detailsClicked(teamId: string): void {
        this.selectedTeamId = teamId;
    }
}
