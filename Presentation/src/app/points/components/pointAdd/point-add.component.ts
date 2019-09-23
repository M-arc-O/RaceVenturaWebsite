import { Component, Input, OnChanges, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { PointDetailViewModel, PointType } from '../../shared';
import { PointComponentBase } from '../../shared/point-component-base.component';
import { addPointSelector, editSelectedPointSelector, IPointsState } from '../../store';
import * as pointActions from '../../store/actions/point.actions';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'app-point-add',
    templateUrl: 'point-add.component.html'
})
export class PointAddComponent extends PointComponentBase implements OnInit, OnChanges, AfterViewInit {
    @Input() public type: AddEditType;
    @Input() public details: PointDetailViewModel;

    public pointTypes = PointType;
    public addEditType = AddEditType;

    private addPointNgForm: NgForm;

    public addPointForm: FormGroup;
    public addBase$: Observable<IBase>;
    public editBase$: Observable<IBase>;

    constructor(
        private store: Store<IPointsState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.addBase$ = this.store.pipe(select(addPointSelector));
        this.editBase$ = this.store.pipe(select(editSelectedPointSelector));
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

    public ngAfterViewInit(): void {
        if (this.details.pointId === undefined) {
            this.addPointForm.controls['type'].setValue(PointType.CheckPoint);
        }
    }

    private setupForm(details?: PointDetailViewModel): void {
        const formBuilder = new FormBuilder();

        let name = '';
        let type = PointType.CheckPoint;
        let value: number;
        let latitude: number;
        let longitude: number;
        let answer = '';
        let message = '';

        if (details !== undefined) {
            name = details.name;
            type = details.type;
            value = details.value;
            latitude = details.latitude;
            longitude = details.longitude;
            answer = details.answer;
            message = details.message;
        }

        this.addPointForm = formBuilder.group({
            name: [name, [Validators.required]],
            type: [type, [Validators.required]],
            value: [value, [Validators.required]],
            latitude: [latitude, [Validators.required]],
            longitude: [longitude, [Validators.required]],
            answer: [answer, []],
            message: [message, []]
        });
    }

    public addPointClick(ngFrom: NgForm): void {
        if (this.addPointForm.valid) {
            this.addPointNgForm = ngFrom;

            const viewModel = new PointDetailViewModel();
            viewModel.stageId = this.details.stageId;
            viewModel.name = this.addPointForm.get('name').value;
            viewModel.type = this.addPointForm.get('type').value;
            viewModel.value = this.addPointForm.get('value').value;
            viewModel.latitude = this.addPointForm.get('latitude').value;
            viewModel.longitude = this.addPointForm.get('longitude').value;
            viewModel.answer = this.addPointForm.get('answer').value;
            viewModel.message = this.addPointForm.get('message').value;

            switch (this.type) {
                case AddEditType.Add:
                    this.store.dispatch(new pointActions.AddPointAction(viewModel));
                    break;
                case AddEditType.Edit:
                    viewModel.pointId = this.details.pointId;
                    this.store.dispatch(new pointActions.EditPointAction(viewModel));
                    break;
            }
        } else {
            this.validateAllFormFields(this.addPointForm);
        }
    }

    public getErrorText(error: HttpErrorResponse): string {
        switch (error.error.toString()) {
            case '1':
                return 'A point with this name already exists in this race.';
            default:
                return 'Default error!';
        }
    }

    private resetForm(): void {
        if (this.addPointNgForm !== undefined) {
            this.addPointNgForm.resetForm();
        }

        if (this.addPointForm !== undefined) {
            this.addPointForm.reset();
            this.addPointForm.controls['type'].setValue(PointType.CheckPoint);
        }
    }
}
