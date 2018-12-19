import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { RaceDetailViewModel } from './models/race-detail-view-model';

export class RaceUtilities {
    public static setupForm(details?: RaceDetailViewModel): FormGroup {
        const formBuilder = new FormBuilder();

        let name = '';
        let checkCoordinates = false;
        let specialTasksAreStage = false;
        let maximumTeamSize;
        let minimumPointsToCompleteStage;
        let startDate;
        let startTime;
        let endDate;
        let endTime;

        if (details !== undefined) {
            name = details.name;
            checkCoordinates = details.coordinatesCheckEnabled;
            specialTasksAreStage = details.specialTasksAreStage;
            maximumTeamSize = details.MaximumTeamSize;
            minimumPointsToCompleteStage = details.MinimumPointsToCompleteStage;
            startDate = undefined;
            startTime = undefined;
            endDate = undefined;
            endTime = undefined;
        }

        return formBuilder.group({
            name: [name, [Validators.required]],
            checkCoordinates: [checkCoordinates],
            specialTasksAreStage: [specialTasksAreStage],
            maximumTeamSize: [maximumTeamSize, [Validators.required]],
            minimumPointsToCompleteStage: [minimumPointsToCompleteStage, [Validators.required]],
            startDate: [startDate],
            startTime: [startTime],
            endDate: [endDate],
            endTime: [endTime],
        });
    }
}
