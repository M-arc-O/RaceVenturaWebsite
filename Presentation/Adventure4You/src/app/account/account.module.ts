import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountRoutingModule } from './account-routing.module';
import { HomeComponent } from './home';

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    AccountRoutingModule
  ],
})
export class AccountModule { }
