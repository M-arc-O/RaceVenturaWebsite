import { Component } from '@angular/core';
import { CarouselService } from '../carousel/carousel.service';

@Component({
    selector: 'app-app-support',
    templateUrl: './app.support.component.html'
})
export class AppSupportComponent {
    constructor(private carouselService: CarouselService) {
        this.carouselService.showCarousel = true;
    }
}
