import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormArray, FormBuilder, Validators } from "@angular/forms";
import { DataService } from '../Services/app.dataService';
import { AppControl } from '../Model/app.author';


@Component({
    selector: 'rf-new',
    templateUrl: 'app.reactiveformNew.html'
})

export class ReactiveFormNewComponent implements OnInit {
    reactiveForm: FormGroup;
    hobbyList: Array<AppControl>;

    constructor(private dataService: DataService) { }

    ngOnInit() {
        this.reactiveForm = new FormGroup({
            'firstname': new FormControl(null, Validators.required),
            'address': new FormGroup({
                'city': new FormControl(null, Validators.required),
                'state': new FormControl(null),
            }),
            'hobbies': new FormArray([])
        });
    }

    onAddHobby() {
        const hobbyControl = new FormControl(null, Validators.required);

        const arrary = (<FormArray>this.reactiveForm.get('hobbies'));
        arrary.push(hobbyControl);

        // -------------------------------------------
        // Sample Code:
        // -------------------------------------------
        // const arr = new FormArray([
        //     new FormControl('Nancy', Validators.minLength(2)),
        //     new FormControl('Drew'),
        // ]);
        // console.log(arr.value);   // ['Nancy', 'Drew']
        // console.log(arr.status);  // 'VALID'
    }

    onListHobbies() {
        this.hobbyList = this.dataService.getHobbies();
        console.log(this.hobbyList);
    }

    submitForm() {
        console.log(this.reactiveForm);
    }

    // submitForm(data: any) {
    //     console.log(data);
    // }
}