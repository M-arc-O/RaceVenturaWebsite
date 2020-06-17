import { PointType } from './point-type';

export class PointDetailViewModel {
    public pointId: string;
    public stageId: string;
    public name: string;
    public type: PointType;
    public value: number;
    public latitude: number;
    public longitude: number;
    public answer: string;
    public message: string;
}
