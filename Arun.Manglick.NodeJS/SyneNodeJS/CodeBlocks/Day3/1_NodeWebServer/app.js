const http = require("http");

var server = http.createServer((req,res)=>{
    console.log(req.headers);
    res.write("<h1>Response from the Server...</h1>");
    res.end();
});

server.listen(3000,function () {
    console.log("Server Started at port 3000....");
});