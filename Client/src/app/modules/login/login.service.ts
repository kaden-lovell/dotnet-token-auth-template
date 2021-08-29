import { Injectable } from "@angular/core";
import { HttpService } from "src/app/services/http/http.service";

@Injectable()
export class LoginService {
    constructor(private readonly _httpService: HttpService) { }
    async login(model: any): Promise<any> {
        const url = `/api/login/login`;
        return await this._httpService.post(url, model);
    }

    async logout(model: any): Promise<any> {
        const url = `/api/login/logout`;
        return await this._httpService.post(url, model);
    }

    async create(model: any): Promise<any> {
        const url = `/api/user/create`;
        return await this._httpService.post(url, model);
    }
}