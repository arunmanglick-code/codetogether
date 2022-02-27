import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { CommonModule } from '@angular/common';

import { RootComponent } from './app.component';
import { ListComponent } from './app.listComponent';
import { DataListComponent } from './app.dataListComponent';
import { CounterComponent } from './app.counterComponent';


@NgModule({
    declarations: [RootComponent,ListComponent,DataListComponent,CounterComponent ],
    imports: [BrowserModule, FormsModule,CommonModule],
    bootstrap: [RootComponent]
})
export class AppModule { }