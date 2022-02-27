import { Component } from "@angular/core";

@Component({
    selector: 'my-app',
    template: `<h2>Root Component</h2>
                <hr>
                <h2>Data Binding</h2>
                <h4>{{message}}</h4>
                <h4 innerHTML = "{{message}}"></h4>                
                <h4 bind-innerHTML = message1></h4>
                <h4 [innerHTML] = message2></h4>

                <hr>
                <h2>Event Binding</h2>
                <button on-click=change()> Click to Change</button>
                <button (click)=resetChange()> Reset Change</button>

                <hr>
                <h2>Explicit Two Way Binding</h2>
                <input type="text" [(ngModel)]="message3">
                <br><br>                
                <input type="text" [value] = message3>

                <hr>
                <h2>Implicit Two Way Binding (Type and press Tab)</h2>
                <input type="text" [(ngModel)]=name (change)=send(name)>
                <h2>Hello {{name}}</h2>
                <hr>
                 `
})
export class RootComponent {
    message:string = "Interpolation";
    message1:string = "Property Binding";
    message2:string = "Property Binding (Box Syntax)";
    message3:string = "Change First Textbox";

    change(){
        this.message = "Message changed at: " + new Date().toLocaleTimeString();
    }

    resetChange(){
        this.message = "Interpolation";
    }

    send(n:string){
        this.message = n.toUpperCase();
    }
}