import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";

import { RootComponent } from './app.component';
import { DataListComponent } from "./app.datalistComponent";

import { CapitalizePipe } from "./Pipes/app.capitalizePipe";
import { FilterDataListPipe } from "./Pipes/app.filterListPipe";

@NgModule({
    imports: [BrowserModule, FormsModule],
    declarations: [RootComponent, DataListComponent, CapitalizePipe,FilterDataListPipe],
    bootstrap: [RootComponent]
})
export class AppModule { }