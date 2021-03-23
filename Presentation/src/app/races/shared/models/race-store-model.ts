import { RaceAccessLevelViewModel } from './race-access-level-view-model';
import { RaceType } from './race-type';
import { RaceViewModel } from './race-view-model';

export class RaceStoreModel extends RaceViewModel {
    public coordinatesCheckEnabled: boolean;
    public raceType: RaceType;
    public allowedCoordinatesDeviation: number;
    public specialTasksAreStage: boolean;
    public maximumTeamSize: number;
    public minimumPointsToCompleteStage: number;
    public penaltyPerMinuteLate: number;
    public pointInformationText: string;
    public startTime: Date;
    public maxDuration: string;
    public accessLevel: RaceAccessLevelViewModel;
}
