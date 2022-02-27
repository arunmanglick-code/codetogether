import { ControlBase } from './control-base';

export class TextboxControl extends ControlBase {
  controlType = 'textbox';
  type: string;

  constructor(objControl: {} = {}) {
    super(objControl);
    this.type = (<any>objControl)['type'] || 'text';
  }
}
