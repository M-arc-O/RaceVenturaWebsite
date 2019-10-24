import { TeamDetailViewModel, StageDetailViewModel } from '.';
import { RaceStoreModel } from './race-store-model';

export class RaceDetailViewModel extends RaceStoreModel {
    public teams: Array<TeamDetailViewModel>;
    public stages: Array<StageDetailViewModel>;
}
