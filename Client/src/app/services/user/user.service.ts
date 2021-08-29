import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { User } from './user';

// shared
import { HttpService } from '../http/http.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  user: User;

  constructor(
    private readonly _httpService: HttpService,
    private readonly _router: Router
  ) {}

  async loadUser() {
    const url = `/api/user/activeuser`;
    return await this._httpService.get(url);
  }

  async login(model: any): Promise<any> {
    const url = `/api/login/login`;
    await this._httpService.post(url, model).then((response) => {
      this.user = response;
      this._router.navigateByUrl('dashboard');
    });
  }

  async logout(): Promise<any> {
    const url = `/api/login/logout`;
    return await this._httpService.get(url).then(() => {
      this._router.navigateByUrl('login');
    });
  }

  async create(model: any): Promise<any> {
    const url = `/api/user/create`;
    return await this._httpService.post(url, model);
  }
}
