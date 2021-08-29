import { Component, OnInit, ViewChild } from '@angular/core';
import { BaseComponent } from '../base/base.component';
import { UserService } from 'src/app/services/user/user.service';

declare let particlesJS: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [UserService],
})
export class LoginComponent extends BaseComponent implements OnInit {
  @ViewChild('form') form: any;

  constructor(readonly _userService: UserService) {
    super(_userService);
  }

  ngOnInit(): void {
    // initialize particle.js library with configuration
    particlesJS.load('particles-js', 'assets/particles.json', function () {
      console.log('callback - particles.js config loaded');
    });
  }

  async login() {
    await this._userService.login(this.model);
  }
}
