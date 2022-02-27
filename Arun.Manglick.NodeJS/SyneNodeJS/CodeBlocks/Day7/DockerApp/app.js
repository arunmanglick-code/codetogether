const express = require('express');

const app = express();

app.get('/', function (req, res) {
  res.send('Hello world\n');
});

app.listen(3000,function(){
    console.log('Server started....');
})