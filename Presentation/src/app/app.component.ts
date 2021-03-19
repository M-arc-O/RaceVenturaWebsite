import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CarouselService } from './components/carousel/carousel.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['app.component.css']
})
export class AppComponent implements OnInit, OnDestroy, AfterViewInit {
  private unsubscribe$ = new Subject<void>();

  public showCarousel = false;

  constructor(
    private carouselService: CarouselService,
    private cd: ChangeDetectorRef) {
  }

  ngOnInit(): void {
    this.carouselService.showCarousel$.pipe(takeUntil(this.unsubscribe$)).subscribe(value => {
      this.showCarousel = value;
      this.cd.detectChanges();
    });
  }

  ngAfterViewInit(): void {
    this.cd.detectChanges();
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
