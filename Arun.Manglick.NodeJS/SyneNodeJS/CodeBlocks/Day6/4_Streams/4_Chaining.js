var fs = require('fs');
var zlib = require('zlib');

fs.createReadStream('file1.txt')
    .pipe(zlib.createGzip())
    .pipe(fs.createWriteStream('file1.txt.gz'));

console.log("Compression Completed....");