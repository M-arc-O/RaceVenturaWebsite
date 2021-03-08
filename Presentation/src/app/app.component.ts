import { Component } from '@angular/core';
import { CarouselService } from './components/carousel/carousel.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['app.component.css']
})
export class AppComponent {
  get showCarousel(): Boolean {
    return this.carouselService.showCarousel;
  }

  constructor(private carouselService: CarouselService) {
  }
}
