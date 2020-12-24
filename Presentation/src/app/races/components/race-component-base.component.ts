import { ComponentBase } from 'src/app/shared';
import { RaceType } from '../shared/models';
import { Directive } from '@angular/core';

@Directive()
export abstract class RaceComponentBase extends ComponentBase {
    public raceTypeToString(type: RaceType): string {
        switch (type) {
            case RaceType.Classic:
                return 'Classic';
            case RaceType.NoTimeLimit:
                return 'No time limit';
            default:
                return 'Unknown type';
        }
    }
}