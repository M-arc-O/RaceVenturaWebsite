import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CarouselService } from 'src/app/components/carousel/carousel.service';
import { UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { RaceAccessLevelViewModel, RaceAccessViewModel, RaceDetailViewModel } from '../../shared/models';
import { accessesSelector, addAccessSelector, deleteAccessSelector, editAccessSelector, IRacesState, loadAccessSelector } from '../../store';
import { RaceComponentBase } from '../race-component-base.component';
import * as raceAccessActions from '../../store/actions/race-access.actions';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'app-race-access',
    templateUrl: './race-access.component.html'
})
export class RaceAccessComponent extends RaceComponentBase implements OnInit, OnChanges {
    @Input() public details: RaceDetailViewModel;

    public raceAccessLevels = RaceAccessLevelViewModel;

    private addRaceAccessNgForm: NgForm;
    public raceAccessForm: FormGroup;
    public editAccessForms = new Array<FormGroup>();

    public raceAccesses$: Observable<RaceAccessViewModel[]>;
    public loadBase$: Observable<IBase>;
    public addBase$: Observable<IBase>;
    public editBase$: Observable<IBase>;
    public deleteBase$: Observable<IBase>;

    constructor(
        private store: Store<IRacesState>,
        private carouselService: CarouselService,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.carouselService.showCarousel$.next(false);
        this.raceAccesses$ = this.store.pipe(select(accessesSelector));
        this.loadBase$ = this.store.pipe(select(loadAccessSelector));
        this.addBase$ = this.store.pipe(select(addAccessSelector));
        this.editBase$ = this.store.pipe(select(editAccessSelector));
        this.deleteBase$ = this.store.pipe(select(deleteAccessSelector));
    }

    public ngOnInit(): void {
        this.setupForm();

        this.raceAccesses$.pipe(takeUntil(this.unsubscribe$)).subscribe(accesses => {
            if (accesses !== undefined) {
                const formBuilder = new FormBuilder();
                this.editAccessForms = new Array<FormGroup>();

                accesses.forEach(access => {
                    let form = formBuilder.group({
                        accessLevel: [access.accessLevel, [Validators.required]],
                    })

                    form.get('accessLevel').valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe(value => {
                        this.accessLevelChanged(access, value);
                    });

                    this.editAccessForms.push(form);
                });

            }
        });

        this.addBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetForm();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.loadBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    public ngOnChanges(changes: SimpleChanges): void {
        if (this.details !== undefined) {
            this.store.dispatch(new raceAccessActions.LoadRaceAccessAction(this.details.raceId));
        }
    }

    private setupForm(): void {
        const formBuilder = new FormBuilder();

        this.raceAccessForm = formBuilder.group({
            email: ["", [Validators.required, Validators.email]],
            accessLevel: [RaceAccessLevelViewModel.Read, [Validators.required]],
        });
    }

    public addRaceAccessClick(ngFrom: NgForm): void {
        if (this.raceAccessForm.valid) {
            this.addRaceAccessNgForm = ngFrom;

            const viewModel = new RaceAccessViewModel();
            viewModel.raceId = this.details.raceId;
            viewModel.userEmail = this.raceAccessForm.get('email').value;
            viewModel.accessLevel = +this.raceAccessForm.get('accessLevel').value;

            console.log(viewModel)

            this.store.dispatch(new raceAccessActions.AddRaceAccessAction(viewModel));
        } else {
            this.validateAllFormFields(this.raceAccessForm);
        }
    }

    public accessLevelChanged(access: RaceAccessViewModel, value: string): void {
        let viewModel = new RaceAccessViewModel();
        viewModel.raceId = access.raceId;
        viewModel.userEmail = access.userEmail;
        viewModel.accessLevel = +value;

        this.store.dispatch(new raceAccessActions.EditRaceAccessAction(viewModel));
    }

    public deleteAccessClick(access: RaceAccessViewModel): void {
        this.store.dispatch(new raceAccessActions.DeleteRaceAccessAction(access));
    }

    public raceAccessLevelToString(type: RaceAccessLevelViewModel): string {
        switch (type) {
            case RaceAccessLevelViewModel.Read:
                return 'Read';
            case RaceAccessLevelViewModel.ReadWrite:
                return 'Read write';
            case RaceAccessLevelViewModel.WriteTeams:
                return 'Write teams';
            default:
                return 'Unknown type';
        }
    }

    public getRaceAccessLevels(): Object {
        return Object.keys(this.raceAccessLevels).filter(e => !isNaN(+e) && +e > 0).map(o => ({ index: +o, name: this.raceAccessLevels[o] }));
    }

    private resetForm(): void {
        if (this.addRaceAccessNgForm !== undefined) {
            this.addRaceAccessNgForm.resetForm();
            this.raceAccessForm.reset();
        }
    }

    public getErrorText(error: HttpErrorResponse): string {
        switch (error.error.toString()) {
            case '4':
                return `No user with email address '${this.raceAccessForm.get('email').value}'`;
            default:
                return 'Default error!';
        }
    }
}
