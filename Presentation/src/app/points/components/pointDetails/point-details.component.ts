import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AddEditType, UserService } from 'src/app/shared';
import { IBase } from 'src/app/store/base.interface';
import { PointDetailViewModel, PointRequest, PointViewModel } from '../../shared';
import { PointComponentBase } from '../../shared/point-component-base.component';
import { IPointsState, loadSelectedPointSelector, selectedPointSelector } from '../../store';
import * as pointActions from '../../store/actions/point.actions';

@Component({
    selector: 'app-point-details',
    templateUrl: 'point-details.component.html'
})
export class PointDetailsComponent extends PointComponentBase implements OnInit, OnChanges {
    @Input() selectedPoint: PointViewModel;

    public pointDetails$: Observable<PointDetailViewModel>;
    public pointDetailsLoad$: Observable<IBase>;

    public addEditType = AddEditType;

    constructor(private store: Store<IPointsState>,
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.pointDetails$ = this.store.pipe(select(selectedPointSelector));
        this.pointDetailsLoad$ = this.store.pipe(select(loadSelectedPointSelector));
    }

    public ngOnInit(): void {
        this.pointDetailsLoad$.pipe(takeUntil(this.unsubscribe$)).subscribe(base => {
            if (base !== undefined && base.error !== undefined) {
                this.handleError(base.error);
            }
        });
    }

    public ngOnChanges(changes: SimpleChanges): void {
        this.store.dispatch(new pointActions.LoadPointDetailsAction(this.getPointRequest()));
    }

    public RemovePointClicked(): void {
        this.store.dispatch(new pointActions.DeletePointAction(this.selectedPoint));
    }

    private getPointRequest(): PointRequest {
        const request = new PointRequest();
        request.pointId = this.selectedPoint.pointId;
        request.stageId = this.selectedPoint.stageId;

        return request;
    }
}
