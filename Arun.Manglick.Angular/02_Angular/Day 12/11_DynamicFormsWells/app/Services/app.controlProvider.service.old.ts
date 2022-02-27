import { Injectable } from '@angular/core';
import { Http, Response, Request, Headers } from "@angular/http";

import { ControlBase } from '../Model/control-base';
import { TextboxControl } from '../Model/control-textbox';
import { DropdownControl } from '../Model/control-dropdown';
import { DropdownOption } from '../Model/app.model';

import { JSONService } from '../Services/app.jsonService';

@Injectable()
export class ControlProviderService {
  listJSON: any[] = [];
  controls: ControlBase[] = [];

  constructor(private jsonService: JSONService,private http: Http) {
    this.jsonService.readJSON().subscribe(
      results => {
        this.listJSON = results;
        console.log('JSON data Inside', this.listJSON.length);

        for (var i = 0; i < this.listJSON.length; i++) {
          let controlObject = this.listJSON;
        }
      })
  }

  getControls() {
    console.log('Controls data Outside', this.controls.length);
    return this.controls.sort((a, b) => a.order - b.order);
  }
}
