import { Component, Input } from '@angular/core';
import { FormGroup }        from '@angular/forms';
import { ControlBase }     from '../../Model/control-base';

@Component({
  selector: 'df-control',
  templateUrl: './dynamic-control.component.html'
})
export class DynamicControlComponent {
  @Input() control: ControlBase;
  @Input() controlFormGroup: FormGroup;

  get isValid() { 
    return this.controlFormGroup.controls[this.control.key].valid; 
  }
}
