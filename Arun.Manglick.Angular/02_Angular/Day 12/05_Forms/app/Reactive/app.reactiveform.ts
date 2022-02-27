import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from "@angular/forms";

@Component({
    selector: 'rf',
    templateUrl: 'app.reactiveform.html'
})

export class ReactiveFormComponent implements OnInit {
    registerForm: FormGroup;

    constructor(private fb: FormBuilder) { }

    ngOnInit() {
        this.registerForm = this.fb.group({
            firstname: [null, Validators.required],
            lastname: [null, Validators.compose([Validators.required, Validators.maxLength(10), Validators.minLength(2)])],
            address: this.fb.group({
                city: [null, Validators.required],
                zip: [0, Validators.required]
            })
        });

        // this.registerForm = this.fb.group({
        //     firstname: null,
        //     lastname: null,
        //     address: this.fb.group({
        //         city: null,
        //         zip: 0
        //     })
        // });
    }

    logForm(data: any) {
        console.log(data);
    }
}