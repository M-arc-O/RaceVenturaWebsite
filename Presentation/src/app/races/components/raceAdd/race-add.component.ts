import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators, FormControl, AbstractControl, ValidatorFn } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { RaceDetailViewModel, RaceType } from '../../shared/models';
import { addRaceSelector, editSelectedRaceSelector, IRacesState } from '../../store';
import * as racesActions from '../../store/actions/race.actions';
import { AddEditType } from '../../../shared';
import { HttpErrorResponse } from '@angular/common/http';
import { timeValidator } from '../../shared/Validators/time.validator';
import { RaceComponentBase } from '../race-component-base.component';

@Component({
    selector: 'app-race-add',
    templateUrl: './race-add.component.html'
})
export class RaceAddComponent extends RaceComponentBase implements OnInit, OnChanges {
    @Input() public type: AddEditType;
    @Input() public details: RaceDetailViewModel;
    
    public raceTypes = RaceType;
    public addEditType = AddEditType;

    private addRaceNgForm: NgForm;

    public addRaceForm: FormGroup;
    public addBase$: Observable<IBase>;
    public editBase$: Observable<IBase>;

    constructor(
        private store: Store<IRacesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.addBase$ = this.store.pipe(select(addRaceSelector));
        this.editBase$ = this.store.pipe(select(editSelectedRaceSelector));
    }

    public ngOnInit(): void {
        this.setupForm(this.details);

        if (this.type === AddEditType.Add) {
            this.addBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
                if (base !== undefined && base.success) {
                    this.resetForm();
                }

                if (base !== undefined && base.error !== undefined) {
                    this.handleError(base.error);
                }
            });
        }

        if (this.type === AddEditType.Edit) {
            this.editBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
                if (base !== undefined && base.error !== undefined) {
                    this.handleError(base.error);
                }
            });
        }
    }

    public ngOnChanges(): void {
        if (this.type === AddEditType.Edit) {
            this.setupForm(this.details);
        }
    }

    private setupForm(details?: RaceDetailViewModel): void {
        const formBuilder = new FormBuilder();

        let name = '';
        let type = RaceType.Classic;
        let checkCoordinates = false;
        let allowedCoordinatesDeviation;
        let specialTasksAreStage = false;
        let maximumTeamSize;
        let minimumPointsToCompleteStage;
        let penaltyPerMinuteLate;
        let pointInformationText;
        let startDate;
        let startTime;
        let endDate;
        let endTime;

        if (details !== undefined) {
            name = details.name;
            type = details.raceType;
            checkCoordinates = details.coordinatesCheckEnabled;
            allowedCoordinatesDeviation = details.allowedCoordinatesDeviation;
            specialTasksAreStage = details.specialTasksAreStage;
            maximumTeamSize = details.maximumTeamSize;
            minimumPointsToCompleteStage = details.minimumPointsToCompleteStage;
            penaltyPerMinuteLate = details.penaltyPerMinuteLate;
            pointInformationText = details.pointInformationText;
            startDate = this.getDateString(details.startTime);
            startTime = this.getTimeString(details.startTime);
            endDate = this.getDateString(details.endTime);
            endTime = this.getTimeString(details.endTime);
        }

        let checkCoordinatesControl = new FormControl(checkCoordinates);
        checkCoordinatesControl.valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe(value => {
            if (value) {
                allowedCoordinatesDeviationControl.clearValidators();
                allowedCoordinatesDeviationControl.setValidators(Validators.required);
            } else {
                allowedCoordinatesDeviationControl.clearValidators();
            }            
        });

        let allowedCoordinatesDeviationControl = new FormControl(allowedCoordinatesDeviation);
        if (checkCoordinates) {
            allowedCoordinatesDeviationControl.setValidators(Validators.required);
        }

        this.addRaceForm = formBuilder.group({
            name: [name, [Validators.required]],
            type: [type],
            checkCoordinates: checkCoordinatesControl,
            allowedCoordinatesDeviation: allowedCoordinatesDeviationControl,
            specialTasksAreStage: [specialTasksAreStage],
            maximumTeamSize: [maximumTeamSize, [Validators.required]],
            minimumPointsToCompleteStage: [minimumPointsToCompleteStage, [Validators.required]],
            penaltyPerMinuteLate: [penaltyPerMinuteLate, [Validators.required]],
            pointInformationText: [pointInformationText, [Validators.required]],
            startDate: [startDate, [Validators.required]],
            startTime: [startTime, [Validators.required, timeValidator(false)]],
            endDate: [endDate, [Validators.required]],
            endTime: [endTime, [Validators.required, timeValidator(false)]],
        });

        this.addRaceForm.get('checkCoordinates').valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe((value) => {
            let validators = [];
            
            if (value) {
                validators = [Validators.required];
            }

            this.resetFormControl(this.addRaceForm.get('allowedCoordinatesDeviation'), validators);
        });

        this.addRaceForm.get('type').valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe((value) => {
            let penaltyValidators = [];
            let dateValidators = [];
            let timeValidators = [];

            if (value === null || value.toString() === RaceType.Classic.toString()) {
                penaltyValidators = [Validators.required];
                dateValidators = [Validators.required];
                timeValidators = [Validators.required, timeValidator(false)];
            }

            this.resetFormControl(this.addRaceForm.get('penaltyPerMinuteLate'), penaltyValidators);
            this.resetFormControl(this.addRaceForm.get('startDate'), dateValidators);
            this.resetFormControl(this.addRaceForm.get('startTime'), timeValidators);
            this.resetFormControl(this.addRaceForm.get('endDate'), dateValidators);
            this.resetFormControl(this.addRaceForm.get('endTime'), timeValidators);           
        });
    }

    public addRaceClick(ngFrom: NgForm): void {
        if (this.addRaceForm.valid) {
            this.addRaceNgForm = ngFrom;

            const viewModel = new RaceDetailViewModel();
            viewModel.name = this.addRaceForm.get('name').value;
            viewModel.raceType = parseFloat(this.addRaceForm.get('type').value);
            viewModel.coordinatesCheckEnabled = this.addRaceForm.get('checkCoordinates').value;
            viewModel.allowedCoordinatesDeviation = parseFloat(this.addRaceForm.get('allowedCoordinatesDeviation').value);
            viewModel.specialTasksAreStage = this.addRaceForm.get('specialTasksAreStage').value;
            viewModel.maximumTeamSize = parseFloat(this.addRaceForm.get('maximumTeamSize').value);
            viewModel.minimumPointsToCompleteStage = parseFloat(this.addRaceForm.get('minimumPointsToCompleteStage').value);
            viewModel.penaltyPerMinuteLate = parseFloat(this.addRaceForm.get('penaltyPerMinuteLate').value);
            viewModel.pointInformationText = this.addRaceForm.get('pointInformationText').value;
            viewModel.startTime = this.getDate(this.addRaceForm.get('startDate').value, this.addRaceForm.get('startTime').value);
            viewModel.endTime = this.getDate(this.addRaceForm.get('endDate').value, this.addRaceForm.get('endTime').value);

            switch (this.type) {
                case AddEditType.Add:
                    this.store.dispatch(new racesActions.AddRaceAction(viewModel));
                    break;
                case AddEditType.Edit:
                    viewModel.raceId = this.details.raceId;
                    this.store.dispatch(new racesActions.EditRaceAction(viewModel));
                    break;
            }
        } else {
            this.validateAllFormFields(this.addRaceForm);
        }
    }

    private resetForm(): void {
        if (this.addRaceNgForm !== undefined) {
            this.addRaceNgForm.resetForm();
            this.addRaceForm.reset();
            this.addRaceForm.controls['type'].setValue(RaceType.Classic);
        }
    }
    
    private resetFormControl(control: AbstractControl, validators: ValidatorFn[]) {
        control.clearValidators();
        control.setErrors(null);
        control.setValidators(validators);
        control.setValue(null);
    }

    public getErrorText(error: HttpErrorResponse): string {
        switch (error.error.toString()) {
            case '1':
                return 'A race with this name already exists in this race.';
            default:
                return 'Default error!';
        }
    }
}
