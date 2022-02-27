import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import {DataService} from './Services/app.dataService'

import { RootComponent } from './app.component';
import { ListComponent } from "./ComponentList/app.listComponent";
import { QuoteComponent } from "./ComponentQuote/app.quoteComponent";

@NgModule({
    imports: [BrowserModule, FormsModule],
    declarations: [RootComponent,ListComponent,QuoteComponent],
    bootstrap: [RootComponent],
    providers: [DataService]
})
export class AppModule { }