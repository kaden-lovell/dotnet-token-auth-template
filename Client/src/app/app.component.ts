import { Component } from '@angular/core';
import { UserService } from './services/user/user.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'Client';

  constructor(
    private readonly _userService: UserService,
    private readonly _router: Router
  ) {}

  ngOnInit() {
    this._userService.loadUser().then((response) => {
      this._userService.user = response;
    });
  }
}
