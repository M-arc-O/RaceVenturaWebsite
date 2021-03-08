import { Component } from '@angular/core';
import { CarouselService } from '../carousel/carousel.service';

@Component({
    selector: 'app-contact',
    templateUrl: './contact.component.html'
})
export class ContactComponent {
    constructor(private carouselService: CarouselService) {
        this.carouselService.showCarousel = true;
    }
}
