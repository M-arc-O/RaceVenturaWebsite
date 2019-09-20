import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { StageViewModel } from '../../shared';
import { deleteStageSelector, IStagesState, loadStagesSelector, stagesListSelector } from '../../store';
import * as stagesActions from '../../store/actions/stage.actions';

@Component({
    selector: 'app-stages-overview',
    templateUrl: './stages-overview.component.html',
    styleUrls: ['./stages-overview.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class StagesOverviewComponent extends ComponentBase implements OnInit {
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
        this.resetSelectedStage();
        this.loadStagesBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.deleteStageBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetSelectedStage();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.getStages();
    }

    resetSelectedStage() {
        this.selectedStage = new StageViewModel();
        this.selectedStage.raceId = this.raceId;
        this.selectedStage.stageId = undefined;
    }

    getStages(): void {
        this.store.dispatch(new stagesActions.LoadStagesAction(this.raceId));
    }

    detailsClicked(stage: StageViewModel): void {
        this.selectedStage = stage;
    }
}
