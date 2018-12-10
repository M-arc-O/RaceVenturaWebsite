import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { ComponentBase } from 'src/app/shared';
import { AddRaceViewModel } from '../../shared/models/add-race-view-model';
import * as racesActions from '../../store/actions/race.actions';
import { IRacesState } from '../../store/racesState.interface';

@Component({
    selector: 'app-add-race',
    templateUrl: './add-race.component.html'
})
export class AddRaceComponent extends ComponentBase implements OnInit {
    addRaceForm: FormGroup;

    constructor(
        private formBuilder: FormBuilder,
        private store: Store<IRacesState>) {
        super();
    }

    ngOnInit(): void {
        this.setupForm();
    }

    setupForm(): void {
        this.addRaceForm = this.formBuilder.group({
            name: ['', [Validators.required]],
            checkCoordinates: [false],
            specialTasksAreStage: [false]
        });
    }

    addRaceClick(): void {
        if (this.addRaceForm.valid) {
            const viewModel = new AddRaceViewModel();
            viewModel.name = this.addRaceForm.get('name').value;
            viewModel.coordinatesCheckEnabled = this.addRaceForm.get('checkCoordinates').value;
            viewModel.specialTasksAreStage = this.addRaceForm.get('specialTasksAreStage').value;

            this.store.dispatch(new racesActions.AddRaceAction(viewModel));
        } else {
            this.validateAllFormFields(this.addRaceForm);
        }
    }
}
