import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../base/base.component';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  providers: [UserService],
})
export class DashboardComponent extends BaseComponent implements OnInit {
  constructor(readonly _userService: UserService) {
    super(_userService);
  }

  ngOnInit(): void {}

  logout() {
    this._userService.logout();
  }
}
