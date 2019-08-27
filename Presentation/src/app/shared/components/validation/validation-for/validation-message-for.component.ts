import { Component, Input, OnInit } from '@angular/core';
import { FormGroupDirective, FormControlName } from '@angular/forms';
import { ValidationMessageService } from '../shared/validation-message.service';
import { Observable } from 'rxjs';

@Component({
    selector: 'validation-message-for',
    templateUrl: 'validation-message-for.component.html',
    providers: [ValidationMessageService]
})
export class ValidationMessageForComponent implements OnInit {
    @Input()
    public field: string;

    public showMessages = false;

    private control: FormControlName;

    public constructor(
        private form: FormGroupDirective,
        private validationMessagesService: ValidationMessageService) {

    }

    public ngOnInit(): void {
        this.form.ngSubmit.subscribe(() => this.onValueChanged());

        this.control = this.form.directives.find(dir => dir.name === this.field) || null;
        if (this.control != null && this.control.statusChanges != null) {
            (<Observable<any>>this.control.statusChanges).subscribe(() => this.onValueChanged());
        }

        this.onValueChanged();
    }

    public onValueChanged(): void {
        if (!this.form || !this.control) {
            this.displayError('');
            return;
        }

        if (!this.control.valid && (this.control.touched || this.control.dirty || this.form.submitted) && this.control.errors) {
            for (const error in this.control.errors) {
                if (error) {
                    this.displayError(error);
                    break;
                }
            }
        } else {
            this.displayError('');
        }
    }

    private displayError(error: string): void {
        let messageSet = false;
        for (const component of this.validationMessagesService.getMessageComponents()) {
            component.showMessage = component.error === error;
            messageSet = messageSet || component.showMessage;
        }
        this.showMessages = messageSet;

        if (!messageSet && error !== '' && error !== 'defaulterrormessage') {
            this.displayError('defaulterrormessage');
        }
    }
}
