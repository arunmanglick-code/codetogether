console.log("Using Third Party Modules");
const lodModule = require('lodash');

console.log(lodModule.isString('Arun'));
const uniqueArray = lodModule.uniq(['Arun',1,'Arun',1,2,3]);
console.log('Array After Duplicate Removal: ', uniqueArray);
