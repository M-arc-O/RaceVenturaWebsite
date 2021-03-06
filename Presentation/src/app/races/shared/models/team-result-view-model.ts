import { StageResultViewModel } from '.';
import { TeamCategory } from './team-category';

export class TeamResultViewModel {
    public teamNumber: number;
    public teamName: string;
    public category: TeamCategory;
    public endTime: Date;
    public raceDuration: string;
    public totalValue: number;
    public numberOfPoints: number;
    public numberOfStages: number;
    public stageResults: StageResultViewModel[];
}
