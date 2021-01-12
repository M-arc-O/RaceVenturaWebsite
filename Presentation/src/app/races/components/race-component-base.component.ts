import { ComponentBase } from 'src/app/shared';
import { RaceType } from '../shared/models';
import { Component } from '@angular/core';

@Component({
    selector: 'app-race-component-base',
    template: ``
})
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