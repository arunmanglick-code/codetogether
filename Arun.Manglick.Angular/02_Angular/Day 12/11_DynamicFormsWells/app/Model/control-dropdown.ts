import { ControlBase } from './control-base';
import { DropdownOption } from './app.model';


export class DropdownControl extends ControlBase {
  controlType = 'dropdown';
  selectOptions: DropdownOption[] = [];   // selectOptions: {key: string, value: string}[] = [];

  constructor(objControl: {} = {}) {
    super(objControl);
    this.selectOptions = (<any>objControl)['selectOptions'] || [];
  }
}
