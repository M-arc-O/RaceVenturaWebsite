import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { StageDetailViewModel } from '../../shared';
import { addStageSelector, editSelectedStageSelector, IStagesState } from '../../store';
import * as stageActions from '../../store/actions/stage.actions';
import { AddEditType } from '../../../shared';

@Component({
    selector: 'app-stage-add',
    templateUrl: './stage-add.component.html'
})
export class StageAddComponent extends ComponentBase implements OnInit, OnChanges {
    @Input() public type: AddEditType;
    @Input() public details: StageDetailViewModel;

    public addEditType = AddEditType;

    private addStageNgForm: NgForm;

    public addStageForm: FormGroup;
    public addBase$: Observable<IBase>;
    public editBase$: Observable<IBase>;

    constructor(
        private store: Store<IStagesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.addBase$ = this.store.pipe(select(addStageSelector));
        this.editBase$ = this.store.pipe(select(editSelectedStageSelector));
    }

    public ngOnInit(): void {
        this.addBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetForm();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.editBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.store.dispatch(new stageActions.LoadStagesAction(this.details.raceId));
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    public ngOnChanges(): void {
        this.setupForm(this.details);
    }

    private setupForm(details?: StageDetailViewModel): void {
        const formBuilder = new FormBuilder();

        let name = '';
        let minimumPointsToCompleteStage;

        if (details !== undefined) {
            name = details.name;
            minimumPointsToCompleteStage = details.mimimumPointsToCompleteStage;
        }

        this.addStageForm = formBuilder.group({
            name: [name, [Validators.required]],
            minimumPointsToCompleteStage: [minimumPointsToCompleteStage, []]
        });
    }

    public addStageClick(ngFrom: NgForm): void {
        if (this.addStageForm.valid) {
            this.addStageNgForm = ngFrom;

            const viewModel = new StageDetailViewModel();
            viewModel.name = this.addStageForm.get('name').value;
            viewModel.mimimumPointsToCompleteStage = this.addStageForm.get('minimumPointsToCompleteStage').value;
            viewModel.raceId = this.details.raceId;

            switch (this.type) {
                case AddEditType.Add:
                    this.store.dispatch(new stageActions.AddStageAction(viewModel));
                    break;
                case AddEditType.Edit:
                    viewModel.id = this.details.id;
                    this.store.dispatch(new stageActions.EditStageAction(viewModel));
                    break;
            }
        } else {
            this.validateAllFormFields(this.addStageForm);
        }
    }

    private resetForm(): void {
        if (this.addStageNgForm !== undefined) {
            this.addStageNgForm.resetForm();
            this.addStageForm.reset();
        }
    }
}
