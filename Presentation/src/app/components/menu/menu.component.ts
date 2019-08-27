import { Component } from '@angular/core';
import { UserService } from 'src/app/shared';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html'
  })
export class MenuComponent {
    get loggedIn(): boolean {
        return this.userService.authToken !== undefined;
    }

    constructor(private userService: UserService) {
    }
}
