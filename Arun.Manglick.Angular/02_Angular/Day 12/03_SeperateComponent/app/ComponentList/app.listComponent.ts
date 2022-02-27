import { Component, OnInit } from '@angular/core';
import { Author } from '../Models/app.author';
import {DataService} from '../Services/app.dataService'

@Component({
    selector: 'my-list',
    templateUrl: 'app.listComponent.html', 
    styles: [require('./app.listComponent.css').toString()]
})

export class ListComponent implements OnInit {
    list: Array<Author>;
    selectedAuthor: Author;
    
    constructor(private dataService: DataService) {}
    // constructor(@Inject(DataService) private dataService: any) {}
    
      ngOnInit() {
        this.list = this.dataService.getAuthors();
      }
      
      selectAuthor(a: Author) {
        this.selectedAuthor = a;
        this.dataService.setAuthor(this.selectedAuthor);
      }
    
      isSelected(a: Author) {
        return this.selectedAuthor === a;
      }    
}