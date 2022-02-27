export class ControlBase {
  key: string;
  value: string;
  label: string;
  required: boolean;
  order: number;

  constructor(objControl: { key?: string, value?: string, label?: string, required?: boolean, order?: number, controlType?: string } = {}) {
    this.key = objControl.key || '';
    this.value = objControl.value || '';
    this.label = objControl.label || '';
    this.required = !!objControl.required;
    this.order = objControl.order === undefined ? 1 : objControl.order;
  }
}
