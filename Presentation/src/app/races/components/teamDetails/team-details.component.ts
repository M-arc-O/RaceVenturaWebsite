import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store';
import { StageStoreModel, TeamStoreModel } from '../../shared/models';
import { ISelectedRace, stagesSelector, editTeamSelector } from '../../store';
import * as teamActions from '../../store/actions';

@Component({
    selector: 'app-team-details',
    templateUrl: './team-details.component.html'
})
export class TeamDetailsComponent extends ComponentBase implements OnInit {
    @Input() selectedTeam: TeamStoreModel;

    public setFinishTimeForm: FormGroup;
    public addEditType = AddEditType;

    public stages$: Observable<StageStoreModel[]>;
    public editBase$: Observable<IBase>;

    constructor(private store: Store<ISelectedRace>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.stages$ = this.store.pipe(select(stagesSelector));
        this.editBase$ = this.store.pipe(select(editTeamSelector));
    }

    ngOnInit(): void {
        this.setupForm();
    }

    private setupForm(): void {
        const formBuilder = new FormBuilder();

        this.setFinishTimeForm = formBuilder.group({
            finishDate: [this.getDateString(new Date())],
            finishTime: ['', Validators.required],
        });
    }

    public setFinishTimeClick(): void {
        if (this.setFinishTimeForm.valid) {
            const viewModel: TeamStoreModel = Object.assign({}, this.selectedTeam);
            viewModel.raceFinished = this.getDate(this.setFinishTimeForm.get('finishDate').value,
                this.setFinishTimeForm.get('finishTime').value);

            this.store.dispatch(new teamActions.EditTeamAction(viewModel));
        } else {
            this.validateAllFormFields(this.setFinishTimeForm);
        }
    }

    public RemoveTeamClicked(): void {
        this.store.dispatch(new teamActions.DeleteTeamAction(this.selectedTeam));
    }
}
