import { AppControl } from "..//Model/app.author";
import { DropdownOption } from "..//Model/app.author";

export class DataService {
  list: Array<AppControl>;

  constructor() {
    var control1 = {
      key:"hobby1",
      caption: "Hobby 1",
      controlType: "textbox",
      required: false,
      options: [
        { key: 'na', value: 'NA' }
      ]
    };

    var control2 = {
      key:"hobby2",
      caption: "Hobby 2",
      controlType: "textbox",
      required: false,
      options: [
        { key: 'solid', value: 'Solid' },
        { key: 'great', value: 'Great' },
        { key: 'good', value: 'Good' },
        { key: 'unproven', value: 'Unproven' }
      ],
    };

    this.list = [control1, control2];
  }

  getHobbies() {
    return this.list;
  }
}