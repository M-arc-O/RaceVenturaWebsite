import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { AddEditType } from '../../../shared';
import { StageStoreModel } from '../../shared/models';
import { addStageSelector, editStageSelector, IStages } from '../../store';
import * as stageActions from '../../store/actions/stage.actions';

@Component({
    selector: 'app-stage-add',
    templateUrl: './stage-add.component.html'
})
export class StageAddComponent extends ComponentBase implements OnInit, OnChanges {
    @Input() public type: AddEditType;
    @Input() public details: StageStoreModel;

    public addEditType = AddEditType;

    private addStageNgForm: NgForm;

    public addStageForm: FormGroup;
    public addBase$: Observable<IBase>;
    public editBase$: Observable<IBase>;

    constructor(
        private store: Store<IStages>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.addBase$ = this.store.pipe(select(addStageSelector));
        this.editBase$ = this.store.pipe(select(editStageSelector));
    }

    public ngOnInit(): void {
        this.setupForm(this.details);

        if (this.type === AddEditType.Add) {
            this.addBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
                if (base !== undefined && base.success) {
                    this.resetForm();
                }

                if (base !== undefined && base.error !== undefined) {
                    if (base.error.status !== 400) {
                        this.handleError(base.error);
                    }
                }
            });
        }

        if (this.type === AddEditType.Edit) {
            this.editBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
                if (base !== undefined && base.error !== undefined) {
                    if (base.error.status !== 400) {
                        this.handleError(base.error);
                    }
                }
            });
        }
    }

    public ngOnChanges(): void {
        if (this.type === AddEditType.Edit) {
            this.setupForm(this.details);
        }
    }

    private setupForm(details?: StageStoreModel): void {
        const formBuilder = new FormBuilder();

        let name = '';
        let number: number;
        let minimumPointsToCompleteStage;

        if (details !== undefined) {
            name = details.name;
            number = details.number;
            minimumPointsToCompleteStage = details.mimimumPointsToCompleteStage;
        }

        this.addStageForm = formBuilder.group({
            name: [name, [Validators.required]],
            number: [number, [Validators.required]],
            minimumPointsToCompleteStage: [minimumPointsToCompleteStage, []]
        });
    }

    public addStageClick(ngFrom: NgForm): void {
        if (this.addStageForm.valid) {
            this.addStageNgForm = ngFrom;

            const viewModel = new StageStoreModel();
            viewModel.name = this.addStageForm.get('name').value;
            viewModel.number = this.addStageForm.get('number').value;
            viewModel.mimimumPointsToCompleteStage = this.addStageForm.get('minimumPointsToCompleteStage').value;
            viewModel.raceId = this.details.raceId;

            switch (this.type) {
                case AddEditType.Add:
                    this.store.dispatch(new stageActions.AddStageAction(viewModel));
                    break;
                case AddEditType.Edit:
                    viewModel.stageId = this.details.stageId;
                    this.store.dispatch(new stageActions.EditStageAction(viewModel));
                    break;
            }
        } else {
            this.validateAllFormFields(this.addStageForm);
        }
    }

    public getErrorText(error: HttpErrorResponse): string {
        switch (error.error.toString()) {
            case '1':
                return 'A stage with this number already exists in this race.';
            default:
                return 'Default error!';
        }
    }

    private resetForm(): void {
        if (this.addStageNgForm !== undefined) {
            this.addStageNgForm.resetForm();
            this.addStageForm.reset();
        }
    }
}
