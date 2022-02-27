const express = require('express');
const app = express();
const port = process.env.PORT || 3000;
// --------------------------------------------
// app.set("view engine","jade");
// app.get("/",(req,res)=>{
//     res.render("jade/index",{title:"Express with Jade View"});
//     //res.render("index.jade",{title:"Express with Jade View"});  // This works when files are immediately below Views Folder
// });

// app.get("/empView",(req,res)=>{
//     let employees = [{id:1,name:"Ram"},
//                      {id:2,name:"RK"},
//                      {id:3,name:"Manish"},
//                      {id:4,name:"Abhijeet"},
//                      {id:5,name:"Varun"}];

//     res.render("jade/emps",{title:"Employees View",empList:employees});
// });

// ---------------------------------------------
app.set("view engine","jade");

app.get("/",(req,res)=>{
    res.render("jade/index",{title:"Express with EJS View"});
});

app.get("/empView",(req,res)=>{
    let employees = [{id:1,name:"Ram"},
                     {id:2,name:"RK"},
                     {id:3,name:"Manish"},
                     {id:4,name:"Abhijeet"},
                     {id:5,name:"Varun"}];

    res.render("jade/emps",{title:"Employees View",empList:employees});
});
// ---------------------------------------------
app.listen(port,()=>{
    console.log("Express Server Started at port....", port);
});
// ---------------------------------------------