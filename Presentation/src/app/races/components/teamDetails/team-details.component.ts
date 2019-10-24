import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { TeamDetailViewModel } from '../../shared/models';
import { ISelectedRace } from '../../store';
import * as teamActions from '../../store/actions/team.actions';

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
