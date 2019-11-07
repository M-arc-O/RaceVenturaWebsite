import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { TeamStoreModel, StageStoreModel } from '../../shared/models';
import { ISelectedRace, stagesSelector } from '../../store';
import * as teamActions from '../../store/actions/team.actions';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-team-details',
    templateUrl: './team-details.component.html'
})
export class TeamDetailsComponent extends ComponentBase {
    @Input() selectedTeam: TeamStoreModel;

    public addEditType = AddEditType;

    public stages$: Observable<StageStoreModel[]>;

    constructor(private store: Store<ISelectedRace>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.stages$ = this.store.pipe(select(stagesSelector));
    }

    public RemoveTeamClicked(): void {
        this.store.dispatch(new teamActions.DeleteTeamAction(this.selectedTeam));
    }
}
