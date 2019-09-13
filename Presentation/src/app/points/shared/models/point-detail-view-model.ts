import { PointViewModel } from './point-view-model';
import { PointType } from './point-type';

export class PointDetailViewModel extends PointViewModel {
    public type: PointType;
    public value: number;
    public coordinates: string;
    public answer: string;
    public message: string;
}
