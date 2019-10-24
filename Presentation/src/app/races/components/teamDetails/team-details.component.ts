import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { TeamDetailViewModel } from '../../shared/models';
import { ISelectedRace, teamsSelector } from '../../store';
import * as teamActions from '../../store/actions/teams.actions';

@Component({
    selector: 'app-team-details',
    templateUrl: './team-details.component.html'
})
export class TeamDetailsComponent extends ComponentBase {
    @Input() selectedTeam: TeamDetailViewModel;

    public addEditType = AddEditType;

    constructor(private store: Store<ISelectedRace>,
        userService: UserService,
        router: Router) {
        super(userService, router);
    }

    public RemoveTeamClicked(): void {
        this.store.dispatch(new teamActions.DeleteTeamAction(this.selectedTeam));
    }
}
