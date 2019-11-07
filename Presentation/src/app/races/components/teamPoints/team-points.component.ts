import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { UserService } from 'src/app/shared';
import { IBase } from 'src/app/store';
import { PointDetailViewModel, TeamPointVisitedViewModel } from '../../shared/models';
import { addPointVisitedSelector, deletePointVisitedSelector, IPoints, pointsSelector, pointsVisitedSelector } from '../../store';
import * as teamPointActions from '../../store/actions/team-point-visited';
import { PointComponentBase } from '../point-component-base.component';


@Component({
    selector: 'app-team-points',
    templateUrl: './team-points.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class TeamPointsComponent extends PointComponentBase implements OnInit {
    @Input() raceId: string;
    @Input() teamId: string;

    public points$: Observable<PointDetailViewModel[]>;
    public pointsVisited$: Observable<TeamPointVisitedViewModel[]>;
    public addBase$: Observable<IBase>;
    public deleteBase$: Observable<IBase>;

    public form: FormGroup;

    private filteredPoints: PointDetailViewModel[];
    private visitedPoints: TeamPointVisitedViewModel[];

    constructor(
        private formBuilder: FormBuilder,
        private store: Store<IPoints>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.points$ = this.store.pipe(select(pointsSelector));
        this.pointsVisited$ = this.store.pipe(select(pointsVisitedSelector));
        this.addBase$ = this.store.pipe(select(addPointVisitedSelector));
        this.deleteBase$ = this.store.pipe(select(deletePointVisitedSelector));
    }

    ngOnInit(): void {
        this.points$.pipe(takeUntil(this.unsubscribe$)).subscribe(points => this.setupForm(points));
        this.pointsVisited$.pipe(takeUntil(this.unsubscribe$)).subscribe(pointsVisited => this.updateForm(pointsVisited));
    }

    private setupForm(points: Array<PointDetailViewModel>): void {
        this.filteredPoints = this.getPoints(points);

        this.form = this.formBuilder.group({
            points: new FormArray([])
        });

        if (this.filteredPoints !== undefined) {
            this.filteredPoints.forEach(point => {
                this.getPointArray().push(new FormControl());
            });
        }

        this.getPointArray().valueChanges.pipe(takeUntil(this.unsubscribe$)).subscribe(val => {
            this.checkboxValueChanged(val);
        });
    }

    private updateForm(pointsVisited: Array<TeamPointVisitedViewModel>): void {
        this.visitedPoints = pointsVisited;
        for (let i = 0; i < this.filteredPoints.length; i++) {
            const visitedPoint = this.visitedPoints.find(vp => vp.pointId === this.filteredPoints[i].pointId);
            const control = this.getPointArray().controls[i] as FormControl;

            if (control !== undefined) {
                control.setValue(visitedPoint !== undefined, { emitEvent: false, onlySelf: true });
            }
        }
    }

    private checkboxValueChanged(val: any): void {
        console.log('Checkbox value changed ' + this.stageId);
        for (let i = 0; i < val.length; i++) {
            const visitedPoint = this.visitedPoints.find(vp => vp.pointId === this.filteredPoints[i].pointId);

            if (val[i] && visitedPoint === undefined) {
                const viewModel = this.getViewModel(i, visitedPoint);
                this.store.dispatch(new teamPointActions.AddTeamPointVisitedAction(viewModel));
            }
            if (!val[i] && visitedPoint !== undefined) {
                const viewModel = this.getViewModel(i, visitedPoint);
                this.store.dispatch(new teamPointActions.DeleteTeamPointVisitedAction(viewModel));
            }
        }
    }

    private getViewModel(index: number, visitedPoint: TeamPointVisitedViewModel): TeamPointVisitedViewModel {
        const viewModel = new TeamPointVisitedViewModel();
        viewModel.raceId = this.raceId;
        viewModel.stageId = this.stageId;
        viewModel.teamId = this.teamId;
        viewModel.pointId = this.filteredPoints[index].pointId;
        viewModel.teamPointVisitedId = visitedPoint !== undefined ? visitedPoint.teamPointVisitedId : undefined;
        return viewModel;
    }

    private getPointArray(): FormArray {
        return (this.form.controls.points as FormArray);
    }
}
