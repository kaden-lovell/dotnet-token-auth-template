import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { retry } from "rxjs/operators";

// shared
@Injectable()
export class HttpService {
    base = 'https://localhost:5001';
    constructor(private readonly _router: Router, private readonly _httpClient: HttpClient) { }

    get(url: string): Promise<any> {
        const result =
            this._httpClient
                .get(this.base + url, { withCredentials: true })
                .pipe(retry(3))
                .toPromise()
                .then(response => {
                    return Promise.resolve(response);
                })
                .catch(err => {
                    if (this.attemptViewDestroyedError(err)) {
                        return Promise.resolve(null);
                    }

                    if (err && err.status === 401) {
                        // this.navigate("/login");
                        return Promise.reject(err);
                    }

                    return Promise.resolve(null);
                });

        return result;
    }

    post(url: string, model = null): Promise<any> {
        const result =
            this._httpClient
                .post(this.base + url, model, { withCredentials: true })
                .toPromise()
                .then(response => {
                    return Promise.resolve(response);
                })
                .catch(err => {
                    if (err && err.status === 401) {
                        // this.navigate("/login");
                        return Promise.reject(err);
                    }

                    return Promise.resolve({});
                });

        return result;
    }

    private attemptViewDestroyedError(err): boolean {
        if (!err.message.startsWith("ViewDestroyedError:")) {
            return false;
        }

        console.error(err.message);

        return true;
    }
}