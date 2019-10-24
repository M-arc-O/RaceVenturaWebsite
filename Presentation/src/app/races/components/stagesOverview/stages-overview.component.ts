import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import * as stagesActions from '../../store/actions/stage.actions';
import { StageDetailViewModel } from '../../shared/models';
import { IStages, stagesSelector, deleteStageSelector } from '../../store';

@Component({
    selector: 'app-stages-overview',
    templateUrl: './stages-overview.component.html',
    styleUrls: ['./stages-overview.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class StagesOverviewComponent extends ComponentBase implements OnInit {
    @Input() raceId: string;

    public stages$: Observable<StageDetailViewModel[]>;
    public deleteStageBase$: Observable<IBase>;
    public selectedStage: StageDetailViewModel;
    public addEditType = AddEditType;

    constructor(
        private store: Store<IStages>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.stages$ = this.store.pipe(select(stagesSelector));
        this.deleteStageBase$ = this.store.pipe(select(deleteStageSelector));
    }

    ngOnInit(): void {
        this.resetSelectedStage();

        this.deleteStageBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetSelectedStage();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    resetSelectedStage() {
        this.selectedStage = new StageDetailViewModel();
        this.selectedStage.raceId = this.raceId;
        this.selectedStage.stageId = undefined;
    }

    detailsClicked(stage: StageDetailViewModel): void {
        this.selectedStage = stage;
    }
}
