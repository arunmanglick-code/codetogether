import { Component, OnInit } from '@angular/core';
import { ControlProviderService } from './Services/app.controlProvider.service';
import { JSONService } from './Services/app.jsonService';

import { ControlBase } from './Model/control-base';
import { TextboxControl } from './Model/control-textbox';
import { DropdownControl } from './Model/control-dropdown';
import { DropdownOption } from './Model/app.model';

@Component({
  selector: 'my-app',
  template: ` <h2>Angular Dynamic Forms Demo</h2>
              <dynamic-form [controls]="controlList"></dynamic-form>
            `
})
export class RootComponent {
  controlList: ControlBase[];
  listJSON: any[];

  constructor(private service: ControlProviderService, private jsonService: JSONService) { }

  ngOnInit() {
    this.controlList = this.service.getControls();    
    // ----------------------------------------------
    // this.jsonService.readJSON().subscribe(
    //   results => {
    //     this.listJSON = results;
    //     console.log('JSON data Inside', this.listJSON.length);
    //   },
    //   (err) => { console.log(err) }
    // );

    // let textControl1: TextboxControl = new TextboxControl({
    //   key: 'firstName',
    //   label: 'First name',
    //   value: 'Bombasto',
    //   required: true,
    //   order: 1
    // });

    // let textControl2: TextboxControl = new TextboxControl({
    //   key: 'emailAddress',
    //   label: 'Email',
    //   required: true,
    //   order: 2,
    //   type: 'email'
    // });

    // let dropdownControl: DropdownControl = new DropdownControl({
    //   key: 'designation',
    //   label: 'Designation',
    //   required: true,
    //   selectOptions: [
    //     { key: 'director', value: 'Director' },
    //     { key: 'manager', value: 'Manager' },
    //     { key: 'architect', value: 'Architect' },
    //     { key: 'developer', value: 'Developer' },
    //     { key: 'tester', value: 'Tester' }
    //   ],
    //   order: 3
    // });

    // let controls: ControlBase[] = [];
    // controls.push(textControl1);
    // controls.push(textControl2);
    // controls.push(dropdownControl);

    // this.controlList = controls.sort((a, b) => a.order - b.order);
  }
}