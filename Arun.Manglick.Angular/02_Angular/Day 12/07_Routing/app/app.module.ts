import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { RouterModule } from "@angular/router";

import { QuoteModule } from "./QuoteModule/app.quoteModule";

import { RootComponent } from './app.component';
import { ListComponent } from "./ListComponent/app.listComponent";

import { routes } from "./app.routes";

@NgModule({
    imports: [BrowserModule, FormsModule, QuoteModule, RouterModule.forRoot(routes)],
    declarations: [RootComponent, ListComponent ],
    bootstrap: [RootComponent]
})
export class AppModule { }