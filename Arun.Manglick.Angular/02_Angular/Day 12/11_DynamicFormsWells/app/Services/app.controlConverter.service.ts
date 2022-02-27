import { Injectable }   from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { ControlBase } from '../Model/control-base';

@Injectable()
export class ControlConverterService {
  constructor() { }

  toFormGroup(controls: ControlBase[] ) {
    let formGroup: any = {};

    controls.forEach(control => {
      formGroup[control.key] = control.required ? new FormControl(control.value || '', Validators.required)
                                              : new FormControl(control.value || '');
    });
    return new FormGroup(formGroup);
  }
}
