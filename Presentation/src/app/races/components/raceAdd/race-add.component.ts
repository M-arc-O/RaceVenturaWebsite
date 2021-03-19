import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CarouselService } from 'src/app/components/carousel/carousel.service';
import { UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { AddEditType } from '../../../shared';
import { RaceDetailViewModel, RaceType } from '../../shared/models';
import { timeValidator } from '../../shared/Validators/time.validator';
import { addRaceSelector, editSelectedRaceSelector, IRacesState } from '../../store';
import * as racesActions from '../../store/actions/race.actions';
import { RaceComponentBase } from '../race-component-base.component';

@Component({
    selector: 'app-race-add',
    templateUrl: './race-add.component.html'
})
export class RaceAddComponent extends RaceComponentBase implements OnInit, OnChanges {
    @Input() public type = AddEditType.Add;
    @Input() public details: RaceDetailViewModel;

    public raceTypes = RaceType;
    public addEditType = AddEditType;

    private addRaceNgForm: NgForm;

    public addRaceForm: FormGroup;
    public addBase$: Observable<IBase>;
    public editBase$: Observable<IBase>;

    constructor(
        private store: Store<IRacesState>,
        private carouselService: CarouselService,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.carouselService.showCarousel$.next(false);
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

        let penaltyValidators = [];
        let dateValidators = [];
        let timeValidators = [];
        let maxDurationHoursValidators = [];
        let maxDurationMinutesValidators = [];
        let allowedDeviationValidators = [];

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
        let maxDurationHours;
        let maxDurationMinutes;

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

            if (details.maxDuration !== null && details.maxDuration !== undefined) {
                let values = details.maxDuration.split(':');
                maxDurationHours = values[0];
                maxDurationMinutes = values[1];
            }
        }

        allowedDeviationValidators = this.SetCoordinateValidators(checkCoordinates, allowedDeviationValidators);
        ({ penaltyValidators, dateValidators, timeValidators, maxDurationHoursValidators, maxDurationMinutesValidators } = this.SetTypeValidators(type, penaltyValidators, dateValidators, timeValidators, maxDurationHoursValidators, maxDurationMinutesValidators));

        this.addRaceForm = formBuilder.group({
            name: [name, [Validators.required]],
            type: [type],
            checkCoordinates: checkCoordinates,
            allowedCoordinatesDeviation: [allowedCoordinatesDeviation, allowedDeviationValidators],
            specialTasksAreStage: [specialTasksAreStage],
            maximumTeamSize: [maximumTeamSize, [Validators.required, Validators.pattern("^[0-9]+$")]],
            minimumPointsToCompleteStage: [minimumPointsToCompleteStage, [Validators.required, Validators.pattern("^[0-9]+$")]],
            penaltyPerMinuteLate: [penaltyPerMinuteLate, penaltyValidators],
            pointInformationText: [pointInformationText, [Validators.required]],
            startDate: [startDate, dateValidators],
            startTime: [startTime, timeValidators],
            maxDurationHours: [maxDurationHours, maxDurationHoursValidators],
            maxDurationMinutes: [maxDurationMinutes, maxDurationMinutesValidators],
        });

        this.addRaceForm.get('checkCoordinates').valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe((value) => {
            allowedDeviationValidators = [];
            allowedDeviationValidators = this.SetCoordinateValidators(value, allowedDeviationValidators);

            this.resetFormControl(this.addRaceForm.get('allowedCoordinatesDeviation'), allowedDeviationValidators);
        });

        this.addRaceForm.get('type').valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe((value) => {
            penaltyValidators = [];
            dateValidators = [];
            timeValidators = [];
            maxDurationHoursValidators = [];
            maxDurationMinutesValidators = [];

            ({ penaltyValidators, dateValidators, timeValidators, maxDurationHoursValidators, maxDurationMinutesValidators } = this.SetTypeValidators(value, penaltyValidators, dateValidators, timeValidators, maxDurationHoursValidators, maxDurationMinutesValidators));

            this.resetFormControl(this.addRaceForm.get('penaltyPerMinuteLate'), penaltyValidators);
            this.resetFormControl(this.addRaceForm.get('startDate'), dateValidators);
            this.resetFormControl(this.addRaceForm.get('startTime'), timeValidators);
            this.resetFormControl(this.addRaceForm.get('maxDurationHours'), maxDurationHoursValidators);
            this.resetFormControl(this.addRaceForm.get('maxDurationMinutes'), maxDurationMinutesValidators);
        });
    }

    private SetCoordinateValidators(value: boolean, allowedDeviationValidators: Validators[]) {
        if (value) {
            allowedDeviationValidators = [Validators.required, Validators.pattern("^[0-9]+$")];
        }

        return allowedDeviationValidators;
    }

    private SetTypeValidators(value: any, penaltyValidators: Validators[], dateValidators: Validators[], timeValidators: Validators[], maxDurationHoursValidators: Validators[], maxDurationMinutesValidators: Validators[]) {
        if (value === null || value.toString() === RaceType.Classic.toString()) {
            penaltyValidators = [Validators.required, Validators.pattern("^[0-9]+$")];
            dateValidators = [Validators.required];
            timeValidators = [Validators.required, timeValidator(false)];
            maxDurationHoursValidators = [Validators.required, Validators.pattern("^[0-9]+$")];
            maxDurationMinutesValidators = [Validators.required, Validators.pattern("^[0-9]+$"), Validators.max(59)];
        }

        if (value !== null && value.toString() === RaceType.TimeLimit.toString()) {
            penaltyValidators = [Validators.required, Validators.pattern("^[0-9]+$")];
            maxDurationHoursValidators = [Validators.required, Validators.pattern("^[0-9]+$")];
            maxDurationMinutesValidators = [Validators.required, Validators.pattern("^[0-9]+$"), Validators.max(59)];
        }
        return { penaltyValidators, dateValidators, timeValidators, maxDurationHoursValidators, maxDurationMinutesValidators };
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

            if (viewModel.raceType === RaceType.NoTimeLimit) {
                viewModel.maxDuration = undefined;
            }
            else {
                viewModel.maxDuration = `${this.addRaceForm.get('maxDurationHours').value}:${this.addRaceForm.get('maxDurationMinutes').value}:00`;
            }

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
            this.addRaceForm.controls['checkCoordinates'].setValue(false);
            this.addRaceForm.controls['specialTasksAreStage'].setValue(false);
        }
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
