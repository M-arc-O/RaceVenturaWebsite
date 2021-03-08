import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { CarouselService } from 'src/app/components/carousel/carousel.service';
import { ComponentBase, UserService } from 'src/app/shared';

@Component({
    selector: 'app-race-result-wrapper',
    templateUrl: './race-result-wrapper.component.html'
})
export class RaceResultWrapperComponent extends ComponentBase implements OnInit {
    public raceId: string;

    constructor(userService: UserService,
        carouselService: CarouselService,
        router: Router,
        private route: ActivatedRoute) {
        super(userService, carouselService, router);
    }

    ngOnInit() {
        this.route.queryParamMap.pipe(takeUntil(this.unsubscribe$)).subscribe(params => {
            this.raceId = params.get('raceid');
        })
    }
}
