import { Component, ViewChild } from "@angular/core";
import { CounterComponent } from './app.counterComponent';

@Component({
    selector: 'my-app',
    template: `<h2>Root Component</h2>
               <h2 class = "alert alert-danger">{{message}}</h2>
               <div *ngIf="loginError" class="alert alert-danger">
                 <strong>Danger!</strong> Indicates a dangerous or potentially negative action.
                </div>
               <my-list></my-list>
               <my-datalist [personDataList]=pList></my-datalist>   
               <my-counter [interval]=10 (onCountReached)="countReachedFunc($event)" #c1></my-counter> 
               <button class="btn btn-danger" (click)=c1.reset()> Reset from Parent </button>            
               `
})
export class RootComponent {
    pList: Array<string> = ["David1", "John1", "Neel1"];
    message: string = "";
    loginError: boolean = false;

    @ViewChild(CounterComponent) counter1: CounterComponent; // Required when you need to call a method/function of component (e.g. my-counter), referred here.

    ngAfterViewInit() {
        // this.counter.interval = 7;
    }

    countReachedFunc(flag: boolean) {
        if (flag) {
            this.message = "Max Count Reached (+/- Button Will Not Be Active Now)";
            this.loginError = true;
        }
        else {
            this.message = "";
            this.loginError = false;
        }
    }
}