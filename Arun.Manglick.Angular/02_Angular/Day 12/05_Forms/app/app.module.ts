import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { RootComponent } from './app.component';
import { ModelBasedFormComponent } from "./ModelBased/app.modelbased";
import { TemplateFormComponent } from "./Templated/app.templateform";
import { ReactiveFormComponent } from "./Reactive/app.reactiveform";
import { ReactiveFormNewComponent } from "./ReactiveForm/app.reactiveformNew";
import {DataService} from './Services/app.dataService'

@NgModule({
    imports: [BrowserModule, FormsModule, ReactiveFormsModule],
    declarations: [RootComponent, ReactiveFormComponent, ReactiveFormNewComponent, TemplateFormComponent, ModelBasedFormComponent],
    bootstrap: [RootComponent],
    providers: [DataService]
})
export class AppModule { }