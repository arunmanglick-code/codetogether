import { Component, OnInit } from '@angular/core';
import { DataService } from '../Services/app.dataService';
import { Author } from '../Models/app.author';

@Component({
  selector: 'my-quoteArea',
  templateUrl: 'app.quoteComponent.html',
  styles: [require('./app.quoteComponent.css').toString()]
})

export class QuoteComponent implements OnInit {
  constructor(private dataService: DataService) { }
  selectedAuthorQ: Author;

  ngOnInit() {
    // this.selectedAuthor = this.dataService.Author;
  }

  getQuote() {
    this.selectedAuthorQ = this.dataService.getAuthor();
  }
}