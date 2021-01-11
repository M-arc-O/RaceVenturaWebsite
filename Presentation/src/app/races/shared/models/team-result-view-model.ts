import { StageResultViewModel } from '.';
import { TimeSpan } from './timespan';

export class TeamResultViewModel {
    public teamNumber: number;
    public teamName: string;
    public endTime: Date;
    public raceDuration: TimeSpan;
    public totalValue: number;
    public numberOfPoints: number;
    public numberOfStages: number;
    public stageResults: StageResultViewModel[];
}
