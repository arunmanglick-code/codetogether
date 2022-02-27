import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { ControlBase } from '../../Model/control-base';
import { ControlConverterService } from '../../Services/app.controlConverter.service';

@Component({
  selector: 'dynamic-form',
  templateUrl: './dynamic-form.component.html',
  providers: [ControlConverterService]
})
export class DynamicFormComponent implements OnInit {
  @Input() controls: ControlBase[] = [];
  dynamicFormGroup: FormGroup;
  payLoad = '';

  constructor(private controlConverterService: ControlConverterService) { }

  ngOnInit() {
    this.dynamicFormGroup = this.controlConverterService.toFormGroup(this.controls);
  }

  onSubmit() {
    this.payLoad = JSON.stringify(this.dynamicFormGroup.value);
    console.log(this.dynamicFormGroup);
  }
}
