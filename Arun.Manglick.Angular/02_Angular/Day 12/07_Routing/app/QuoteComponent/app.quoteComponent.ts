import { Component } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

import { Author } from "../Models/app.author";
import { DataService } from "../Services/app.dataService";

@Component({
    selector: 'quote',
    templateUrl: 'app.quoteComponent.html',
    styles: [require('./app.quoteComponent.css').toString()]
})

export class QuoteComponent {
    private aname: string;
    sAuthor: Author;
    constructor(private dService: DataService, private route: ActivatedRoute) { }

    ngOnInit() {
        this.route.params.map(p => p['name']).subscribe((n) => { this.aname = n; });
    }

    get() {
        this.sAuthor = this.dService.getAuthor();
    }
}