const express = require('express');
const path = require('path');
const app = express();
const port = process.env.PORT || 3000;
// --------------------------------------------------------
// app.use(function (req,res,next) {
//     console.log("Middelware 1");
//     next();
// });

// app.use(function (req,res,next) {
//     console.log("Middelware 2");
//     //next();
// });
// --------------------------------------------------------
//app.use(express.static(__dirname + '/public'));
app.use(express.static(path.join(__dirname,'public')));
// --------------------------------------------------------
app.get("/",(req,res)=>{
    // res.send("<h1>Hello from Express</h1>");
    // res.sendFile(__dirname + "/public/index.html");
     let employees = [{id:1,name:"Ram"},
                     {id:2,name:"RK"},
                     {id:3,name:"Manish"},
                     {id:4,name:"Abhijeet"},
                     {id:5,name:"Varun"}];
                     
    res.sendFile("/index.html");
});
// --------------------------------------------------------
app.get("/data",(req,res)=>{
    let employees = [{id:1,name:"Ram"},
                     {id:2,name:"RK"},
                     {id:3,name:"Manish"},
                     {id:4,name:"Abhijeet"},
                     {id:5,name:"Varun"}];
    
    const _ = require("underscore");
    let sorted = _.sortBy(employees,'name');

    res.send(sorted);
});
// --------------------------------------------------------
// app.listen(3000);
app.listen(port,()=>{
    console.log("Express Server Started at port....", port);
});
// --------------------------------------------------------