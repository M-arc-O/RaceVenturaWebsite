import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { ComponentBase, UserService } from 'src/app/shared';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html'
  })
export class MenuComponent extends ComponentBase {
    public loggedIn = false;

    constructor(userService: UserService,
        router: Router) {
        super(userService, router);

        this.userService.loggedIn$.pipe(takeUntil(this.unsubscribe$)).subscribe(value => {
            this.loggedIn = value;
        });
    }
}
