var fs = require('fs');
var data = '';

var readStream = fs.createReadStream('file1.txt');
console.log(readStream.hightWaterMark);

readStream.setEncoding('UTF8');

readStream.on('error', function (err) {
    console.log(err);
});

readStream.on('data', function (chunk) {
    console.log("Data Event");
    data += chunk;
});

readStream.on('end', function (chunk) {
    console.log(data);
});

console.log("Completed....");