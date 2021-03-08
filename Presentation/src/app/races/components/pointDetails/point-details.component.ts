import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { CarouselService } from 'src/app/components/carousel/carousel.service';
import { AddEditType, UserService } from 'src/app/shared';
import { PointDetailViewModel } from '../../shared/models';
import { IPoints } from '../../store';
import * as pointActions from '../../store/actions/point.actions';
import { PointComponentBase } from '../point-component-base.component';

@Component({
    selector: 'app-point-details',
    templateUrl: 'point-details.component.html'
})
export class PointDetailsComponent extends PointComponentBase {
    @Input() selectedPoint: PointDetailViewModel;

    public addEditType = AddEditType;

    constructor(private store: Store<IPoints>,
        userService: UserService,
        carouselService: CarouselService,
        router: Router) {
        super(userService, carouselService, router);
    }

    public RemovePointClicked(): void {
        this.store.dispatch(new pointActions.DeletePointAction(this.selectedPoint));
    }
}
