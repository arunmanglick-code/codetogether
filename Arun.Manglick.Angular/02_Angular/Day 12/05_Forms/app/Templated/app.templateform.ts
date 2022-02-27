import { Component } from '@angular/core';

@Component({
    selector: 'tf',
    templateUrl: 'app.templateform.html'
})

export class TemplateFormComponent {
    logForm(data: any) {
        console.log(data);
    }
}