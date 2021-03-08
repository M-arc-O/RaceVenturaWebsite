import { Component } from '@angular/core';
import { CarouselService } from '../carousel/carousel.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    constructor(private carouselService: CarouselService) {
        this.carouselService.showCarousel = true;
    }
}
