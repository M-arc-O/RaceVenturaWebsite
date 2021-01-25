import { HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnChanges, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { AddEditType } from '../../../shared';
import { TeamStoreModel, TeamCategory } from '../../shared/models';
import { addTeamSelector, editTeamSelector, ITeams } from '../../store';
import * as teamActions from '../../store/actions/team.actions';
import { TeamComponentBase } from '../team-component-base.component';

@Component({
    selector: 'app-team-add',
    templateUrl: './team-add.component.html'
})
export class TeamAddComponent extends TeamComponentBase implements OnInit, OnChanges, AfterViewInit {
    @Input() public type: AddEditType;
    @Input() public raceId: string;
    @Input() public details: TeamStoreModel;

    public categorys = TeamCategory;
    public addEditType = AddEditType;

    private addTeamNgForm: NgForm;

    public addTeamForm: FormGroup;
    public addBase$: Observable<IBase>;
    public editBase$: Observable<IBase>;

    constructor(
        private store: Store<ITeams>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.addBase$ = this.store.pipe(select(addTeamSelector));
        this.editBase$ = this.store.pipe(select(editTeamSelector));
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
        if (this.details === undefined) {
            this.addTeamForm.controls['category'].setValue(TeamCategory.Man);
        }
    }

    private setupForm(details?: TeamStoreModel): void {
        const formBuilder = new FormBuilder();

        let name = '';
        let number: number;
        let category: number;

        if (details !== undefined) {
            name = details.name;
            number = details.number;
            category = details.category;
        }

        this.addTeamForm = formBuilder.group({
            name: [name, [Validators.required]],
            number: [number, [Validators.required, Validators.pattern('^[0-9]*$'), Validators.min(1)]],
            category: [category, [Validators.required]]
        });
    }

    public addTeamClick(ngFrom: NgForm): void {
        if (this.addTeamForm.valid) {
            this.addTeamNgForm = ngFrom;

            const viewModel = new TeamStoreModel();
            viewModel.name = this.addTeamForm.get('name').value;
            viewModel.number = parseFloat(this.addTeamForm.get('number').value);
            viewModel.category = parseFloat(this.addTeamForm.get('category').value);
            viewModel.raceId = this.raceId;

            switch (this.type) {
                case AddEditType.Add:
                    this.store.dispatch(new teamActions.AddTeamAction(viewModel));
                    break;
                case AddEditType.Edit:
                    viewModel.teamId = this.details.teamId;
                    this.store.dispatch(new teamActions.EditTeamAction(viewModel));
                    break;
            }
        } else {
            this.validateAllFormFields(this.addTeamForm);
        }
    }

    public getErrorText(error: HttpErrorResponse): string {
        switch (error.error.toString()) {
            case '1':
                return 'A team with this name or number already exists in this race.';
            default:
                return 'Default error!';
        }
    }    

    private resetForm(): void {
        if (this.addTeamNgForm !== undefined) {
            this.addTeamNgForm.resetForm();
            this.addTeamForm.reset();
            this.addTeamForm.get('category').setValue(TeamCategory.Man);
        }
    }
}
