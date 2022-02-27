import { Component, ViewChild } from "@angular/core";

@Component({
    selector: 'my-app',
    template: ` <h2>Pipes Demo - Over Strings & Date</h2>
                <h4>{{name | uppercase}}</h4>
                <h4>{{name | capitalize}}</h4> 
                <h4>{{today}}</h4>
                <h4>{{today | date}}</h4>
                <h4>{{today | date:'MM/dd/yy'}}</h4>
                <h4>{{today | date:format}}</h4>
                <button (click)=toggleFormat()>Toggle</button>
                <hr>
                <h2>Pipes Demo - Over Data List</h2>
                <my-dlist [personList]=plist></my-dlist>
                `
})
export class RootComponent {
    plist = ["Ramakant", "Abhijeet", "Manish", "Subodh"];
    today = new Date();
    toggle = true;
    name = "manish sharma";

    get format() { 
        return this.toggle ? 'shortDate' : 'fullDate'; 
    };

    toggleFormat() {
        this.toggle = !this.toggle;
    }
}