import { Component, Input, OnInit } from '@angular/core';
import { ValidationMessageService } from '../shared/validation-message.service';

@Component({
    selector: 'validation-message',
    templateUrl: 'validation-message.component.html'
})
export class ValidationMessageComponent implements OnInit {
    @Input()
    public error: string;

    public showMessage = false;

    public constructor(private validationMessageService: ValidationMessageService) {
    }

    public ngOnInit(): void {
        this.validationMessageService.addMessageComponent(this);
    }
}
