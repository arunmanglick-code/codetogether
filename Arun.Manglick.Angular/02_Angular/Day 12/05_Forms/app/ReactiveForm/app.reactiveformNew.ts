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

    constructor(private dataService: DataService, private formBuilder: FormBuilder) { }

    ngOnInit() {
        this.reactiveForm = new FormGroup({
            'firstname': new FormControl(null, Validators.required),
            'address': new FormGroup({
                'city': new FormControl(null, Validators.required),
                'state': new FormControl(null),
            }),
            'hobbies': new FormArray([]),
            'hobbiesNew': new FormArray([]),
            items: this.formBuilder.array([this.createItemArray()])
            // items: new FormArray([this.createItem()])
        });
    }
    // ------------------------------------------------------------------------------------
    onAddHobby() {
        const formControl = new FormControl(null, Validators.required);

        const array = (<FormArray>this.reactiveForm.get('hobbies'));
        array.push(formControl);

        // const arr = new FormArray([
        //     new FormControl('Nancy', Validators.minLength(2)),
        //     new FormControl('Drew'),
        // ]);
        // console.log(arr.value);   // ['Nancy', 'Drew']
        // console.log(arr.status);  // 'VALID'
    }

    onListHobbies() {
        const array = (<FormArray>this.reactiveForm.get('hobbiesNew'));
        const list = this.dataService.getHobbies();
        for (let hobby of list) {
            const hobbyControl = new FormControl(null, Validators.required);
            array.push(hobbyControl);
        }
    }
    // ------------------------------------------------------------------------------------
    createItem() {
        return new FormArray([
            new FormControl('name'),
            new FormControl('description'),
            new FormControl('price')
        ]);
    }

    createItemArray() {
        return this.formBuilder.group({
            name: '',
            description: '',
            price: ''
        });
    }
    // ------------------------------------------------------------------------------------
    submitForm() {
        console.log(this.reactiveForm);
    }
}