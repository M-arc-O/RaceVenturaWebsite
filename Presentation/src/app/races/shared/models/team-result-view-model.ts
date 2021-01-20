import { StageResultViewModel } from '.';

export class TeamResultViewModel {
    public teamNumber: number;
    public teamName: string;
    public endTime: Date;
    public raceDuration: string;
    public totalValue: number;
    public numberOfPoints: number;
    public numberOfStages: number;
    public stageResults: StageResultViewModel[];
}
