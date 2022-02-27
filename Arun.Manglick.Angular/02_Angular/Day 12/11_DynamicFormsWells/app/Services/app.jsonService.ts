import { Http, Response, Request, Headers } from "@angular/http";
import { Injectable } from "@angular/core";

@Injectable()
export class JSONService {
    constructor(private http: Http) { }

    readJSON() {
        return this.http.get('assets/controls.json').map((res: Response) => {
            console.log("JSON data from Service" + res.json());
            return res.json();
        });
    }
}