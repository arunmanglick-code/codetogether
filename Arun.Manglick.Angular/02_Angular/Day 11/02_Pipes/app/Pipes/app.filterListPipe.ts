import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filerDataList'
})

export class FilterDataListPipe implements PipeTransform {
    transform(value: string[], args = ""): any {
        // return value.filter( item => item.startsWith(args));
        value = value.map(function(x){ return x.toUpperCase() })
        return value.filter( item => item.startsWith(args.toUpperCase()));
    }
}