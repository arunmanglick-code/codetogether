var employees = [
    {id:1, EmpName: "Employee 1"},
    {id:2, EmpName: "Employee 2"},
    {id:3, EmpName: "Employee 3"},
    {id:4, EmpName: "Employee 4"},
    {id:5, EmpName: "Employee 5"}
];

const arr = require('underscore');

arr.each(employees, function(emp){
    console.log(emp.EmpName);
})