import { Component } from "@angular/core";

@Component({
    selector: 'my-app',
    template: `<h2>Root Component</h2>
                <my-app-2></my-app-2>
                 `
})
export class RootComponent {
}