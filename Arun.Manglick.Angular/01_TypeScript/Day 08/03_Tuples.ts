// Tuple
let myTuple:[string, number]= ["ABC", 10];
let myTuple1:[string, number]= ["ABC", 10, 20];
let myTuple2:[string, number]= ["ABC", 10, 20, "DEF",];  // first two need to be in order, rest can be in any order

function Getdata()
{
    return {name:"Arun", age:20, city:"TN"};
}

function GetdataUsingTuple()
{
    return ["Name", 20, "City"]; // Return Tuple (as an array)
}

