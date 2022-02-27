import { Injectable } from "@angular/core";
import { Http, Response, Request, RequestOptions, Headers } from "@angular/http";
import {Post} from "../Model/app.model";

@Injectable()
export class DataService {
    // private urlJSON = "https://jsonplaceholder.typicode.com/posts";
    private urlJSON = "assets/post.json";

    constructor(private http: Http) { }

    getAllPosts() {
        var headers = new Headers();
        headers.append('Authorization', 'MyCheck');

        return this.http.get(this.urlJSON, {
            headers: headers
        }).map((res:Response) => {
            console.log("mock data" + res.json());
            return res.json();
        });
    }

    // Not Working as Could not post data to a local json in Angular
    createPost(post: Post) {
        console.log("Post Model:", post);
        return this.http.post(this.urlJSON, post, this.jwt()).map(
            (response: Response) => response.json());
    }

    private jwt() {
        var headers = new Headers();
        headers.append('Authorization', 'MyCheck');
        headers.append('Content-Type', 'application/json');

        return new RequestOptions({ headers: headers });
    }
}