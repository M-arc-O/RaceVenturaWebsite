import { StageDetailViewModel } from 'src/app/stages/shared';
import { TeamDetailViewModel } from '.';
import { RaceStoreModel } from './race-store-model';

export class RaceDetailViewModel extends RaceStoreModel {
    public teams: Array<TeamDetailViewModel>;
    public stages: Array<StageDetailViewModel>;
}
