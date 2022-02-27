// var buffer = new Buffer(10);
// var buffer = new Buffer([10,20,30,40,50]);
// var buffer = new Buffer("Just for Fun.","utf-8");
// console.log("Result: " + buffer.toString());

// var buffer = new Buffer(2);
// var len = buffer.write("Just for Fun.");

// console.log("Octets Written : " + len);
// console.log("Result: " + buffer.toString());
// console.log("Result: " + buffer.toString('utf-8', 0, len));

var express = require('express'),
fs = require('fs'),
app = express();

app.get("/",(req,res)=>{
    res.send("<h1>Hello from Express Buffer Session..</h1>");
});

app.get("/:fname", function (req, res) {
    var filePath = "/files/" + req.params.fname + ".pdf";

    fs.readFile(__dirname + filePath, function (err, data) {
        console.log(data);
        res.contentType("application/pdf");
        res.send(data);
    });
})

app.listen(3000, function () {
    console.log("Server Started. at port 3000...");
});
