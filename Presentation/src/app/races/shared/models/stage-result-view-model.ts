import { PointResultViewModel } from '.';

export class StageResultViewModel {
    public stageNumber: number;
    public stageName: string;
    public totalValue: number;
    public maxNumberOfPoints: number;
    public numberOfPoints: number;
    public pointResults: PointResultViewModel[];
}
