var fs = require('fs');
var data = 'This is the text written by WriteStream.';

var writeStream = fs.createWriteStream('file2.txt');

writeStream.write(data, 'UTF8');
writeStream.end();

writeStream.on('finish', function () {
    console.log("Write Completed...");
});

writeStream.on('error', function (err) {
    console.log(err);
});

console.log("Completed....");