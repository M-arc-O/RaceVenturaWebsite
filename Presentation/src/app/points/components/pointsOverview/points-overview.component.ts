import { Component, Input, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { PointViewModel } from '../../shared';
import { deletePointSelector, IPointsState, loadPointsSelector, pointsListSelector } from '../../store';
import * as pointActions from '../../store/actions/point.actions';

@Component({
    selector: 'app-points-overview',
    templateUrl: './points-overview.component.html',
    styleUrls: ['./points-overview.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class PointsOverviewComponent extends ComponentBase implements OnInit {
    @Input() stageId: string;

    public points$: Observable<PointViewModel[]>;
    public loadPointsBase$: Observable<IBase>;
    public deletePointBase$: Observable<IBase>;
    public selectedPoint: PointViewModel;
    public addEditType = AddEditType;

    constructor(
        private store: Store<IPointsState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.points$ = this.store.pipe(select(pointsListSelector));
        this.loadPointsBase$ = this.store.pipe(select(loadPointsSelector));
        this.deletePointBase$ = this.store.pipe(select(deletePointSelector));
    }

    ngOnInit(): void {
        this.resetSelectedPoint();

        this.loadPointsBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.deletePointBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetSelectedPoint();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });

        this.getPoints();
    }

    private resetSelectedPoint() {
        this.selectedPoint = new PointViewModel();
        this.selectedPoint.stageId = this.stageId;
        this.selectedPoint.pointId = undefined;
    }

    getPoints(): void {
        this.store.dispatch(new pointActions.LoadPointsAction(this.stageId));
    }

    detailsClicked(point: PointViewModel): void {
        this.selectedPoint = point;
    }
}
