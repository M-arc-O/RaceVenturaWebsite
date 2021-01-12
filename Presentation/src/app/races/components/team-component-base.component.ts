import { TeamCategory } from "../shared/models";
import { ComponentBase } from "src/app/shared";
import { Component } from "@angular/core";

@Component({
    selector: 'app-team-component-base',
    template: ``
})
export abstract class TeamComponentBase extends ComponentBase {
    public categoryToString(category: TeamCategory): string {
        switch (category) {
            case TeamCategory.Man:
                return 'Man';
            case TeamCategory.Woman:
                return 'Woman';
            case TeamCategory.Mixed:
                return 'Mixed';
            default:
                return 'Unknown type';
        }
    }
}