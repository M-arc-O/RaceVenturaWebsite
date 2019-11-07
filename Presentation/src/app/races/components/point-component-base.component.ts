import { ComponentBase } from 'src/app/shared';
import { PointType, PointDetailViewModel } from '../shared/models';
import { Input } from '@angular/core';

export abstract class PointComponentBase extends ComponentBase {
    @Input() stageId: string;

    public getPoints(points: Array<PointDetailViewModel>): Array<PointDetailViewModel> {
        return points.filter(point => point.stageId === this.stageId);
    }

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
