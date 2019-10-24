import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { StageStoreModel } from '../../shared/models';
import { IStages } from '../../store';
import * as stageActions from '../../store/actions/stage.actions';

@Component({
    selector: 'app-stage-details',
    templateUrl: './stage-details.component.html'
})
export class StageDetailsComponent extends ComponentBase {
    @Input() selectedStage: StageStoreModel;

    public addEditType = AddEditType;

    constructor(private store: Store<IStages>,
        userService: UserService,
        router: Router) {
        super(userService, router);
    }

    public RemoveStageClicked(): void {
        this.store.dispatch(new stageActions.DeleteStageAction(this.selectedStage));
    }
}
