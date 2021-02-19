import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { ComponentBase, UserService } from "src/app/shared";

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html'
})
export class ForgotPasswordComponent extends ComponentBase implements OnInit {    
    public forgotPasswordForm: FormGroup;
    public requestSend: boolean;

    constructor(
        userService: UserService,
        router: Router) {
        super(userService, router);
        this.requestSend = false;
    }

    ngOnInit(): void {
        this.setupForm();
    }

    public submitClick(): void {
        if (this.forgotPasswordForm.valid) {
            this.userService.forgotPassword(this.forgotPasswordForm.get("emailAddress").value);
            this.requestSend = true;
        } else {
            this.validateAllFormFields(this.forgotPasswordForm);
        }
    }

    private setupForm(): void {
        const formBuilder = new FormBuilder();

        this.forgotPasswordForm = formBuilder.group({
            emailAddress: ["", [Validators.required, Validators.email]]
        });
    }
}
