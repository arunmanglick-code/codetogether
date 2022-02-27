import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'my-list',
    template: `<h2>List Component - Three ways:</h2>
                Person Selected: {{personSelected}}
                <!--
                <ul>
                    <li template="ngFor let person of personList">
                        <a href="#" (click)="showPerson1(person)">{{person}}</a>
                    </li>
                </ul>   
                <hr>
                
                <ul>                
                    <ng-template ngFor let-person [ngForOf]="personList">
                        <li>
                            <a href="#" (click)="showPerson1(person)">{{person}}</a>
                        </li>
                    </ng-template>
                </ul>  
                --> 
                <ul>                
                    <li *ngFor="let person of personList">
                        <a href="http://www.google.com" (click)="showPerson($event, person)">{{person}}</a>
                    </li>
                </ul>   
    `
})

export class ListComponent implements OnInit {
    personList: Array<string>;
    personSelected: string ;

    constructor() {
        this.personList = ["David", "John", "Neel"]
    }

    ngOnInit() { }

    showPerson(e:Event, n: string) {
        this.personSelected = n;
        e.preventDefault();
    }
    
    showPerson1(n: string) {
        this.personSelected = n;
    }
}