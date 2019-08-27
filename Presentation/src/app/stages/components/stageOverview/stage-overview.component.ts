import { ChangeDetectionStrategy, Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { StageViewModel } from '../../shared/';
import { deleteStageSelector, IStagesState, loadStagesSelector, stagesListSelector } from '../../store';
import * as stagesActions from '../../store/actions/stage.actions';
import { AddEditType } from '../../../shared';

@Component({
    selector: 'app-stage-overview',
    templateUrl: './stage-overview.component.html',
    styleUrls: ['./stage-overview.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class StageOverviewComponent extends ComponentBase implements OnInit {
    @Input() raceId: string;

    public stages$: Observable<StageViewModel[]>;
    public loadStagesBase$: Observable<IBase>;
    public deleteStageBase$: Observable<IBase>;
    public selectedStage: StageViewModel;
    public addEditType = AddEditType;

    constructor(
        private store: Store<IStagesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.stages$ = this.store.pipe(select(stagesListSelector));
        this.loadStagesBase$ = this.store.pipe(select(loadStagesSelector));
        this.deleteStageBase$ = this.store.pipe(select(deleteStageSelector));
    }

    ngOnInit(): void {
        this.loadStagesBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.deleteStageBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.selectedStage = undefined;
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.getStages();
    }

    getStages(): void {
        this.store.dispatch(new stagesActions.LoadStagesAction(this.raceId));
    }

    detailsClicked(stage: StageViewModel): void {
        this.selectedStage = stage;
    }
}
