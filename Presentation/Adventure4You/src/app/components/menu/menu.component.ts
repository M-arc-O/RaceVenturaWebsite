import { Component } from '@angular/core';
import { UserService } from 'src/app/shared';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.css']
  })
export class MenuComponent {
    get loggedIn(): boolean {
        return this.userService.loggedIn;
    }

    constructor(private userService: UserService) {
    }
}
