import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import {DataService} from './Services/app.dataService'

import { RootComponent } from './app.component';

@NgModule({
    imports: [BrowserModule, FormsModule],
    declarations: [RootComponent],
    bootstrap: [RootComponent],
    providers: [DataService]
})
export class AppModule { }