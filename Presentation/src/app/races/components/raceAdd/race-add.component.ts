import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { RaceDetailViewModel } from '../../shared';
import { addRaceSelector, editSelectedRaceSelector, IRacesState } from '../../store';
import * as racesActions from '../../store/actions/race.actions';
import { AddEditType } from '../../../shared';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'app-race-add',
    templateUrl: './race-add.component.html'
})
export class RaceAddComponent extends ComponentBase implements OnInit, OnChanges {
    @Input() public type: AddEditType;
    @Input() public details: RaceDetailViewModel;

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
        this.addBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetForm();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.editBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    public ngOnChanges(): void {
        this.setupForm(this.details);
    }

    private setupForm(details?: RaceDetailViewModel): void {
        const formBuilder = new FormBuilder();

        let name = '';
        let checkCoordinates = false;
        let specialTasksAreStage = false;
        let maximumTeamSize;
        let minimumPointsToCompleteStage;
        let penaltyPerMinuteLate;
        let startDate;
        let startTime;
        let endDate;
        let endTime;

        if (details !== undefined) {
            name = details.name;
            checkCoordinates = details.coordinatesCheckEnabled;
            specialTasksAreStage = details.specialTasksAreStage;
            maximumTeamSize = details.maximumTeamSize;
            minimumPointsToCompleteStage = details.minimumPointsToCompleteStage;
            penaltyPerMinuteLate = details.penaltyPerMinuteLate;
            startDate = undefined;
            startTime = undefined;
            endDate = undefined;
            endTime = undefined;
        }

        this.addRaceForm = formBuilder.group({
            name: [name, [Validators.required]],
            checkCoordinates: [checkCoordinates],
            specialTasksAreStage: [specialTasksAreStage],
            maximumTeamSize: [maximumTeamSize, [Validators.required]],
            minimumPointsToCompleteStage: [minimumPointsToCompleteStage, [Validators.required]],
            penaltyPerMinuteLate: [penaltyPerMinuteLate, [Validators.required]],
            startDate: [startDate],
            startTime: [startTime],
            endDate: [endDate],
            endTime: [endTime],
        });
    }

    public addRaceClick(ngFrom: NgForm): void {
        if (this.addRaceForm.valid) {
            this.addRaceNgForm = ngFrom;

            const viewModel = new RaceDetailViewModel();
            viewModel.name = this.addRaceForm.get('name').value;
            viewModel.coordinatesCheckEnabled = this.addRaceForm.get('checkCoordinates').value;
            viewModel.specialTasksAreStage = this.addRaceForm.get('specialTasksAreStage').value;
            viewModel.maximumTeamSize = this.addRaceForm.get('maximumTeamSize').value;
            viewModel.minimumPointsToCompleteStage = this.addRaceForm.get('minimumPointsToCompleteStage').value;
            viewModel.penaltyPerMinuteLate = this.addRaceForm.get('penaltyPerMinuteLate').value;
            viewModel.startTime = undefined;
            viewModel.endTime = undefined;

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
