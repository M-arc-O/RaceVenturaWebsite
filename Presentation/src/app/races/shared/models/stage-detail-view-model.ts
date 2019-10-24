import { StageStoreModel } from './stage-store-model';
import { PointDetailViewModel } from './point-detail-view-model';

export class StageDetailViewModel extends StageStoreModel {
    public points: Array<PointDetailViewModel>;
}
