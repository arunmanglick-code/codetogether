import { Component, OnInit } from '@angular/core';

import { Author } from "../Models/app.author";
import { DataService } from "../Services/app.dataService";

@Component({
    selector: 'my-list',
    templateUrl: 'app.listComponent.html',
    styles: [require('./app.listComponent.css').toString()]
})

export class ListComponent implements OnInit {
    list: Array<Author>;
    selectedAuthor: Author;
  
    constructor(private dService: DataService) { }    
    // constructor(@Inject(DataService) private dService: any) { }
  
    ngOnInit() {
      this.list = this.dService.getAuthors();
    }
  
    selectAuthor(a: Author) {
      this.selectedAuthor = a; //this.dService.getAuthor();      
      this.dService.setAuthor(a);
    }
  
    isSelected(a: Author) {
      return this.selectedAuthor === a;
    }
}