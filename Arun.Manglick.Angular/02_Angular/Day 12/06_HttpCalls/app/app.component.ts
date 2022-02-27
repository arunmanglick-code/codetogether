import { Component, OnInit } from '@angular/core';
import { DataService } from "./Services/app.dataService";
import { Post } from "./Model/app.model"


@Component({
  selector: 'my-app',
  template: ` <h2>HTTP Communication</h2>
              <button (click)=getData()> Get Data </button> 
              <button (click)=postData()> Post Data </button> 
              <ul class="list-unstyled" *ngIf="posts && posts.length">
                <li *ngFor="let post of posts">
                  <h3>{{post.id}}</h3>
                  <h4>{{post.title}}</h4>
                  <p>{{post.body}}</p>
                  <hr/>
                </li>
              </ul>
              
            `,
  providers: [DataService]
})
export class RootComponent implements OnInit {
  posts: Post[];
  post: Post = {
    userId: 100,
    id: 1,
    title: "Fowler",
    body: "Any fool can write code that a computer can understand. Good programmers write code that humans can understand."
  };

  constructor(private dService: DataService) { }

  ngOnInit() {
    // this.dService.getAllPosts().subscribe(
    //   results => this.posts = results,
    //   (err) => { console.log(err) });
  }

  getData() {
    this.dService.getAllPosts().
      subscribe(
      results => {
        this.posts = results;
      },
      (err) => { console.log(err) });
  }

  postData() {
    this.dService.createPost(this.post).
      subscribe(
      results => {
        this.posts = results;
        console.log("Results:", results);
      },
      (err) => { console.log(err) });
  }
}