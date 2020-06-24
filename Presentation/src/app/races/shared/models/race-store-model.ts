import { RaceViewModel } from './race-view-model';

export class RaceStoreModel extends RaceViewModel {
    public coordinatesCheckEnabled: boolean;
    public allowedCoordinatesDeviation: number;
    public specialTasksAreStage: boolean;
    public maximumTeamSize: number;
    public minimumPointsToCompleteStage: number;
    public penaltyPerMinuteLate: number;
    public startTime: Date;
    public endTime: Date;
}
