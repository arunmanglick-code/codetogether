import { Injectable } from '@angular/core';

import { ControlBase } from '../Model/control-base';
import { TextboxControl } from '../Model/control-textbox';
import { DropdownControl } from '../Model/control-dropdown';
import { DropdownOption } from '../Model/app.model';

import { JSONService } from '../Services/app.jsonService';

@Injectable()
export class ControlProviderService {

  constructor() { }

  getControls() {  
    let controls: ControlBase[] = [
      new TextboxControl({
        key: 'firstName',
        label: 'First name',
        value: 'Bombasto',
        required: true,
        order: 1
      }),
      new TextboxControl({
        key: 'emailAddress',
        label: 'Email',
        required: true,
        order: 2,
        type: 'email'
      }),
      new DropdownControl({
        key: 'designation',
        label: 'Designation',
        required: true,
        selectOptions: [
          { key: 'director', value: 'Director' },
          { key: 'manager', value: 'Manager' },
          { key: 'architect', value: 'Architect' },
          { key: 'developer', value: 'Developer' },
          { key: 'tester', value: 'Tester' }
        ],
        order: 3
      })
    ];

    return controls.sort((a, b) => a.order - b.order);
  }
}
