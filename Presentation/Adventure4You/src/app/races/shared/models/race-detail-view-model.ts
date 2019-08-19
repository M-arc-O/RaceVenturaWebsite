import { RaceViewModel } from './race-view-model';

export class RaceDetailViewModel extends RaceViewModel {
    public coordinatesCheckEnabled: boolean;
    public specialTasksAreStage: boolean;
    public maximumTeamSize: number;
    public minimumPointsToCompleteStage: number;
    public penaltyPerMinuteLate: number;
    public startTime: Date;
    public endTime: Date;
}
