import { ValidatorFn, AbstractControl } from "@angular/forms";

export function timeValidator(checkSeconds: boolean): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
        var value = control.value as string;
        
        if (value === null) {
            return null;
        }

        var splitLength = checkSeconds ? 3: 2;
        var hoursAndMinuts = value.split(':');
        var hoursAndMinutsCorrect = false;
        var secondsCorrect = !checkSeconds;

        if (hoursAndMinuts.length === splitLength) {
            if (!isNaN(+hoursAndMinuts[0]) && !isNaN(+hoursAndMinuts[1])) {
                var hours = Number.parseInt(hoursAndMinuts[0]);
                var minutes = Number.parseInt(hoursAndMinuts[1]);
    
                if (hours !== undefined && hours >= 0 && hours <= 23 &&
                    minutes !== undefined && minutes >= 0 && minutes <= 59) {
                    hoursAndMinutsCorrect = true;
                }
            }

            if (checkSeconds && !isNaN(+hoursAndMinuts[2])) {
                var seconds = Number.parseInt(hoursAndMinuts[2]);
    
                if (seconds !== undefined && seconds >= 0 && seconds <= 59) {
                    secondsCorrect = true;
                }
            }
        }

        return hoursAndMinutsCorrect && secondsCorrect ? null: { 'incorrectTime': { value: control.value } };
    };
}