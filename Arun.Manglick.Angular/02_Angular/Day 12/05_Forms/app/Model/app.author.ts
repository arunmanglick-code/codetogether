export interface Author {
    name: string;
    quote: string;
}

export interface AppControl {
    key:string
    caption: string;
    controlType: string;
    required: boolean;
    options:DropdownOption[];
}

export interface DropdownOption {
    key: string;
    value: string;
}
