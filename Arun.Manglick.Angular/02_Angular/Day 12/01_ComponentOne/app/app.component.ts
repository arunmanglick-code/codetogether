import { Component } from '@angular/core';
import { Author } from './Models/app.author';


@Component({
  selector: 'my-app',
  template: ` <h2>Service Demo</h2>
                <hr/>
                <div class="options">
                  <div class="options">
                    <a class="{{isSelected(author) && 'selected' || ''}}" 
                    *ngFor="let author of list" (click)=selectAuthor(author)>{{author.name}}</a>
                  </div>
                  <div>
                    <textarea class="padtop" readonly 
                    *ngIf="selectedAuthor">{{selectedAuthor.quote}}</textarea>
                  </div>
                </div>
                `,
  styles: [require('./app.component.css').toString()]
})
export class RootComponent {
  list: Array<Author>;
  selectedAuthor: Author;

  constructor() {
    var fowler = {
      name: "Fowler",
      quote: "Any fool can write code that a computer can understand. Good programmers write code that humans can understand."
    };
    
    var twain = {
      name: "Twain",
      quote: "Why, I have known clergymen, good men, kind-hearted, liberal, sincere, and all that, who did not know the meaning of a 'flush.' It is enough to make one ashamed of one's species."
    };

    var poe = {
      name: "Poe",
      quote: "Deep into that darkness peering, long I stood there, wondering, fearing, doubting, dreaming dreams no mortal ever dared to dream before."
    };

    var plato = {
      name: "Plato",
      quote: "All things will be produced in superior quantity and quality, and with greater ease, when each man works at a single occupation, in accordance with his natural gifts, and at the right moment, without meddling with anything else. "
    };

    this.list = [twain, fowler, poe, plato];
  }

  selectAuthor(a: Author) {
    this.selectedAuthor = a;
  }

  isSelected(a: Author) {
    return this.selectedAuthor === a;
  }
}