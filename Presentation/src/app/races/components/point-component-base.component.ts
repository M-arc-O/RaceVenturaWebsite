import { ComponentBase } from 'src/app/shared';
import { PointType } from '../shared/models';

export abstract class PointComponentBase extends ComponentBase {
    public pointTypeToString(type: PointType): string {
        switch (type) {
            case PointType.CheckPoint:
                return 'Checkpoint';
            case PointType.SelfyCheckPoint:
                return 'Selfy point';
            case PointType.SpecialTask:
                return 'Special task';
            case PointType.QuestionCheckPoint:
                return 'Question point';
            default:
                return 'Unknown type';
        }
    }
}
