import { Component, OnInit, Inject } from '@angular/core';
import { Author } from './Models/app.author';
import { DataService } from './Services/app.dataService'

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

export class RootComponent implements OnInit {
  list: Array<Author>;
  selectedAuthor: Author;

  constructor(private dataService: DataService) { }
  // constructor(@Inject(DataService) private dataService: any) {}

  ngOnInit() {
    this.list = this.dataService.getAuthors();
  }

  selectAuthor(a: Author) {
    this.selectedAuthor = a;
  }

  isSelected(a: Author) {
    return this.selectedAuthor === a;
  }
}