import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";

import { RootComponent } from './app.component';
import { ComponentOne } from './app.componentOne';
import { ComponentTwo } from './app.ComponentTwo';

@NgModule({
    declarations: [RootComponent, ComponentOne, ComponentTwo],
    imports: [BrowserModule, FormsModule],
    bootstrap: [RootComponent, ComponentOne]
})
export class AppModule { }