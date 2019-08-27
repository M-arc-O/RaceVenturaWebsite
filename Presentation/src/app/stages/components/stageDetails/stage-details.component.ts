import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { StageDetailViewModel } from '../../shared';
import * as stageActions from '../../store/actions/stage.actions';
import { AddEditType } from '../../../shared';
import { IStagesState, selectedStageSelector, loadSelectedStageSelector } from '../../store';

@Component({
    selector: 'app-stage-details',
    templateUrl: './stage-details.component.html'
})
export class StageDetailsComponent extends ComponentBase implements OnInit, OnChanges {
    @Input() stageId: string;

    public stageDetails$: Observable<StageDetailViewModel>;
    public stageDetailsLoad$: Observable<IBase>;

    public addEditType = AddEditType;

    constructor(private store: Store<IStagesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.stageDetails$ = this.store.pipe(select(selectedStageSelector));
        this.stageDetailsLoad$ = this.store.pipe(select(loadSelectedStageSelector));
    }

    public ngOnInit(): void {
        this.stageDetailsLoad$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    public ngOnChanges(changes: SimpleChanges): void {
        this.store.dispatch(new stageActions.LoadStageDetailsAction(this.stageId));
    }

    public RemoveStageClicked(): void {
        this.store.dispatch(new stageActions.DeleteStageAction(this.stageId));
    }
}
