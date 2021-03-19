import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class CarouselService {
    public showCarousel$ = new BehaviorSubject<boolean>(false);
}