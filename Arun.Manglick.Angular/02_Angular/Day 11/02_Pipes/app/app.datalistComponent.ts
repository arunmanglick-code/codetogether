import { Component, Input } from '@angular/core';

@Component({
    selector: 'my-dlist',
    template: ` <input type="text" [(ngModel)]="sortFormat" >
                <h4>Selected Person: {{selectedPerson}}</h4>

                <ul>
                    <li *ngFor="let person of personList | filerDataList:sortFormat ">
                        <a href="http://www.google.com" (click)=select($event,person)>{{person}}</a>
                    </li>
                </ul>      
            `
})

export class DataListComponent {
    @Input() personList: Array<string>;
    selectedPerson: string;
    sortFormat:string;

    select(e: Event, p: string) {
        this.selectedPerson = p;
        e.preventDefault();
    }
}