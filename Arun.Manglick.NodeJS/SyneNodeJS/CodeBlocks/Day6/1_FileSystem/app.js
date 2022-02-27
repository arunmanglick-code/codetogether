var fs = require('fs');

//-------------------------------------------Reading
// fs.readFile('file1.txt','utf-8', function (err,data) {  
//     if(err) throw err;

//     console.log(data);
// });

// var data = fs.readFileSync('file1.txt','utf-8');
// console.log(data);
// console.log("Completed......");

//-------------------------------------------Writing
// fs.writeFile('file2.txt', 'Hello from Node App', 'utf-8', function (err) {  
//     if(err) 
//         console.log(err);
//     else    
//         console.log("Write Operation Completed....");
// });

// fs.appendFile('file2.txt', '\nHello from Node App', 'utf-8', function (err) {  
//     if(err) 
//         console.log(err);
//     else    
//         console.log("Append Operation Completed....");
// });

//-------------------------------------------Open & Read Buffer
fs.open('file1.txt', 'r', function (err, data) {
    if (err) {
        return console.error(err);
    }

    var buffer = new Buffer(15);

    fs.read(data, buffer, 0, buffer.length, 0, function (err, bytes) {
        if (err) throw err;

        if (bytes > 0) {
            console.log(buffer.slice(0, bytes).toString());
        }

        fs.close(data, function (err) {
            if (err) throw err;
        });
    })
});
// ---------------------------------------------