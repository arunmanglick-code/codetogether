import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule }      from '@angular/http';

import { RootComponent } from './app.component';
import { DynamicFormComponent } from './Component/DynamicForm/dynamic-form.component';
import { DynamicControlComponent } from './Component/DynamicControl/dynamic-control.component';

import { JSONService } from './Services/app.jsonService';
import { ControlProviderService } from './Services/app.controlProvider.service'
import { ControlConverterService } from './Services/app.controlConverter.service';

@NgModule({
    declarations: [RootComponent,DynamicFormComponent,DynamicControlComponent],
    imports: [BrowserModule, FormsModule, ReactiveFormsModule,HttpModule],
    bootstrap: [RootComponent],
    providers: [JSONService,ControlProviderService, ControlConverterService]
})
export class AppModule { }