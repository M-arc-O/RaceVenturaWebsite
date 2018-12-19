import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { AddRaceViewModel } from '../../shared/models/add-race-view-model';
import * as racesActions from '../../store/actions/race.actions';
import { addRaceSelector } from '../../store/races.interface';
import { IRacesState } from '../../store/racesState.interface';
import { RaceUtilities } from '../../shared';

@Component({
    selector: 'app-add-race',
    templateUrl: './add-race.component.html'
})
export class AddRaceComponent extends ComponentBase implements OnInit {
    private addRaceNgForm: NgForm;

    public addRaceForm: FormGroup;
    public addBase$: Observable<IBase>;

    constructor(
        private store: Store<IRacesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.addBase$ = this.store.pipe(select(addRaceSelector));
    }

    public ngOnInit(): void {
        this.addRaceForm = RaceUtilities.setupForm();
        this.addBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base.success) {
                this.resetForm();
            }

            if (base !== undefined && base.error !== undefined) {
                console.log(base.error.json());
                this.handleError(base.error);
            }
        });
    }

    public addRaceClick(ngFrom: NgForm): void {
        if (this.addRaceForm.valid) {
            this.addRaceNgForm = ngFrom;

            const viewModel = new AddRaceViewModel();
            viewModel.name = this.addRaceForm.get('name').value;
            viewModel.coordinatesCheckEnabled = this.addRaceForm.get('checkCoordinates').value;
            viewModel.specialTasksAreStage = this.addRaceForm.get('specialTasksAreStage').value;
            viewModel.maximumTeamSize = this.addRaceForm.get('maximumTeamSize').value;
            viewModel.minimumPointsToCompleteStage = this.addRaceForm.get('minimumPointsToCompleteStage').value;
            viewModel.startTime = undefined;
            viewModel.endTime = undefined;

            this.store.dispatch(new racesActions.AddRaceAction(viewModel));
        } else {
            this.validateAllFormFields(this.addRaceForm);
        }
    }

    private resetForm(): void {
        this.addRaceForm.reset();
        this.addRaceNgForm.resetForm();
    }
}
