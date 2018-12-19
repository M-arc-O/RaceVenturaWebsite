import { RaceViewModel } from './race-view-model';

export class RaceDetailViewModel extends RaceViewModel {
    public coordinatesCheckEnabled: boolean;
    public specialTasksAreStage: boolean;
    public MaximumTeamSize: number;
    public MinimumPointsToCompleteStage: number;
    public StartTime: Date;
    public EndTime: Date;
}
