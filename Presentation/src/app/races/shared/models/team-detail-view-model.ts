import { TeamStoreModel } from './team-store-model';
import { TeamPointVisitedViewModel } from './team-point-visited-view-model';

export class TeamDetailViewModel extends TeamStoreModel {
    public pointsVisited: TeamPointVisitedViewModel;
}
