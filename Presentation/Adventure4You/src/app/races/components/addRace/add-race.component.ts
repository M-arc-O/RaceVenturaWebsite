import { Component, OnInit } from '@angular/core';
import { ComponentBase } from 'src/app/shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    selector: 'app-add-race',
    templateUrl: './add-race.component.html'
})
export class AddRaceComponent extends ComponentBase implements OnInit {
    addRaceForm: FormGroup;

    constructor(private formBuilder: FormBuilder) {
        super();
    }

    ngOnInit(): void {
        this.setupForm();
    }

    setupForm(): void {
        this.addRaceForm = this.formBuilder.group({
            name: ['', [Validators.required]],
            checkCoordinates: ['', [Validators.required]],
            specialTasksAreStage: ['', [Validators.required]]
        });
    }

    addRaceClick(): void {
        if (this.addRaceForm.valid) {

        } else {
            this.validateAllFormFields(this.addRaceForm);
        }
    }
}
