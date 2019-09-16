import { PointViewModel } from './point-view-model';
import { PointType } from './point-type';

export class PointDetailViewModel extends PointViewModel {
    public type: PointType;
    public value: number;
    public latitude: number;
    public longitude: number;
    public answer: string;
    public message: string;
}
