import { ValidationMessageComponent } from '../validation-message/validation-message.component';

export class ValidationMessageService {
    private messageComponents: ValidationMessageComponent[];

    public constructor() {
        this.messageComponents = [];
    }

    public addMessageComponent(messageComponent): void {
        this.messageComponents.push(messageComponent);
    }

    public getMessageComponents(): ValidationMessageComponent[] {
        return this.messageComponents;
    }
}
