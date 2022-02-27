import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { RootComponent } from './app.component';
import { ListComponent } from "./ListComponent/app.listComponent";
import { QuoteComponent } from "./QuoteComponent/app.quoteComponent";

@NgModule({
    imports: [BrowserModule, FormsModule],
    declarations: [RootComponent, ListComponent, QuoteComponent],
    bootstrap: [RootComponent]
})
export class AppModule { }