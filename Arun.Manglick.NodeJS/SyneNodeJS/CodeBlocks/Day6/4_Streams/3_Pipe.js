var fs = require('fs');

var readStream = fs.createReadStream('file1.txt');
var writeStream = fs.createWriteStream("file3.txt");
readStream.pipe(writeStream);

console.log("Completed....");