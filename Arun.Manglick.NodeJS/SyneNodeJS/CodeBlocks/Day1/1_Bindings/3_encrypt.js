import Crypto from 'crypto';

function EncryptTokenWithZeroBuffer(secretKey, data) {
    try {
        const passwordHash = Crypto.createHash('md5').update(secretKey, 'utf-8').digest('hex').toUpperCase();
        const iv = new Buffer.alloc(16); // fill with zeros
        const cipher = Crypto.createCipheriv('aes-256-cbc', passwordHash, iv);
        return cipher.update(data, 'utf8', 'hex') + cipher.final('hex');
    } catch (exception) {
        return exception;
    }
}

function Display(a, b) {
    console.log (a + b);
};

Display(2,4);

var res=EncryptTokenWithZeroBuffer("mykey", "Hellow World");
console.log(res);

