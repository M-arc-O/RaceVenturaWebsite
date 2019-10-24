import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import * as stageActions from '../../store/actions/stage.actions';
import { StageDetailViewModel } from '../../shared/models';
import { IStages } from '../../store';

@Component({
    selector: 'app-stage-details',
    templateUrl: './stage-details.component.html'
})
export class StageDetailsComponent extends ComponentBase {
    @Input() selectedStage: StageDetailViewModel;

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
