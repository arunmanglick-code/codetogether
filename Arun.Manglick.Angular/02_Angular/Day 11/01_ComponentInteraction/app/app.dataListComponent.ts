import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'my-datalist',
    template: `<h2>Data List Component</h2>
                Person Selected: {{personSelected}}               
                <ul>                
                    <li *ngFor="let person of personDataList">
                        <a href="http://www.google.com" (click)="showPerson($event, person)">{{person}}</a>
                    </li>
                </ul>   
    `
})

export class DataListComponent implements OnInit {
    @Input() personDataList: Array<string>;
    personSelected: string ;

    constructor() { 
    }

    ngOnInit() { }

    showPerson(e:Event, n: string) {
        this.personSelected = n; 
        e.preventDefault();  // This is to avoid link to go to 'google.com'
    }
}