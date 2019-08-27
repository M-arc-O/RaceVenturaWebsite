import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ValidationMessageComponent, ValidationMessageForComponent } from '.';
import { ValidationMessageService } from './shared/validation-message.service';

@NgModule({
    declarations: [
      ValidationMessageComponent,
      ValidationMessageForComponent
    ],
    imports: [
      BrowserModule
    ],
    exports: [
      ValidationMessageComponent,
      ValidationMessageForComponent
    ]
  })
  export class ValidationModule { }
