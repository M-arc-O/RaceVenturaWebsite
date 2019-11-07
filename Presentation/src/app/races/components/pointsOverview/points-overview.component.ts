import { ChangeDetectionStrategy, Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, ComponentBase, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { PointDetailViewModel } from '../../shared/models';
import { deletePointSelector, IPoints, pointsSelector } from '../../store';
import { PointComponentBase } from '../point-component-base.component';

@Component({
    selector: 'app-points-overview',
    templateUrl: './points-overview.component.html',
    styleUrls: ['./points-overview.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class PointsOverviewComponent extends PointComponentBase implements OnInit, OnChanges {
    public points$: Observable<PointDetailViewModel[]>;
    public deletePointBase$: Observable<IBase>;
    public selectedPoint: PointDetailViewModel;
    public addEditType = AddEditType;

    constructor(
        private store: Store<IPoints>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.points$ = this.store.pipe(select(pointsSelector));
        this.deletePointBase$ = this.store.pipe(select(deletePointSelector));
    }

    ngOnInit(): void {
        this.resetSelectedPoint();

        this.deletePointBase$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.success) {
                this.resetSelectedPoint();
            }

            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    ngOnChanges(changes: SimpleChanges): void {
        this.resetSelectedPoint();
    }

    private resetSelectedPoint() {
        this.selectedPoint = new PointDetailViewModel();
        this.selectedPoint.stageId = this.stageId;
        this.selectedPoint.pointId = undefined;
    }

    detailsClicked(point: PointDetailViewModel): void {
        this.selectedPoint = point;
    }
}
