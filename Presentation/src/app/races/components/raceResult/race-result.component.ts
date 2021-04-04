import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from 'src/app/shared';
import { TeamCategory, TeamResultViewModel } from '../../shared/models';
import { resultStateSelector } from '../../store';
import * as raceActions from '../../store/actions/race.actions';
import { TeamComponentBase } from '../team-component-base.component';

@Component({
    selector: 'app-race-result',
    templateUrl: './race-result.component.html'
})
export class RaceResultComponent extends TeamComponentBase implements OnInit {
    @Input() raceId: string;
    
    public selectedCategory = '4';
    public categorys = TeamCategory;
    public categoryForm: FormGroup;

    public raceResult$: Observable<TeamResultViewModel[]>;

    constructor(private store: Store<TeamResultViewModel[]>,
        private modalService: NgbModal,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.raceResult$ = this.store.pipe(select(resultStateSelector));
    }

    public ngOnInit(): void {
        this.setupForm();
        this.store.dispatch(new raceActions.GetRaceResultAction(this.raceId));
    }

    public refresh(): void {
        this.store.dispatch(new raceActions.GetRaceResultAction(this.raceId));
    }

    public openModal(content: any): void {
        this.modalService.open(content, { size: 'xl', scrollable: true });
    }

    public showOnMapClicked(): void {
        this.router.navigate(['/raceMap', this.raceId]);
    }

    public getResults(value: TeamResultViewModel[]): TeamResultViewModel[] {
        if (this.selectedCategory === '4') {
            return value;
        }
        return value.filter(result => result.category.toString() === this.selectedCategory);
    }

    public getRaceDurationString(value: string): string {
        if (value !== null && value !== undefined) {
            let daysAndRest = value.split(`.`);
            let timeArray = new Array(3);
            let days = '00';

            switch (daysAndRest.length) {
                case 3:
                    days = daysAndRest[0];
                    timeArray = daysAndRest[1].split(':');
                    break; 
                default:
                    timeArray = daysAndRest[0].split(':');
                break;
            }

            return `${days}-${timeArray[0]}-${timeArray[1]}-${timeArray[2]}`;
        }

        return "";
    }

    private setupForm(): void {
        const formBuilder = new FormBuilder();

        this.categoryForm = formBuilder.group({
            category: ['4', [Validators.required]]
        });

        this.categoryForm.get('category').valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe(value => {
            this.selectedCategory = value;
        });
    }
}
