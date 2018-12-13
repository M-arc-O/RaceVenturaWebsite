import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store, select } from '@ngrx/store';
import { ComponentBase, UserService } from 'src/app/shared';
import { AddRaceViewModel } from '../../shared/models/add-race-view-model';
import * as racesActions from '../../store/actions/race.actions';
import { IRacesState } from '../../store/racesState.interface';
import { Router } from '@angular/router';
import { addRaceSelector } from '../../store/races.interface';
import { Observable } from 'rxjs';
import { IBase } from 'src/app/store/base.interface';
import { takeUntil } from 'rxjs/operators';

@Component({
    selector: 'app-add-race',
    templateUrl: './add-race.component.html'
})
export class AddRaceComponent extends ComponentBase implements OnInit {
    public addRaceForm: FormGroup;
    public addBase$: Observable<IBase>;

    constructor(
        private formBuilder: FormBuilder,
        private store: Store<IRacesState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.addBase$ = this.store.pipe(select(addRaceSelector));
    }

    public ngOnInit(): void {
        this.setupForm();
        this.addBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    private setupForm(): void {
        this.addRaceForm = this.formBuilder.group({
            name: ['', [Validators.required]],
            checkCoordinates: [false],
            specialTasksAreStage: [false]
        });
    }

    public addRaceClick(): void {
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
