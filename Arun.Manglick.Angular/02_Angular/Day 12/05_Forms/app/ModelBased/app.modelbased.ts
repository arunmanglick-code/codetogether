import { Component } from '@angular/core';
import { IPerson } from "./app.person";

@Component({
    selector: 'mb',
    templateUrl: 'app.modelbased.html'
})

export class ModelBasedFormComponent {
    person: IPerson = {
        firstname: "David",
        lastname: "Scott",
        address: {
            city: "New Jersey",
            zip: 12345
        }
    };

    logForm() {
        console.log(this.person);
    }
}