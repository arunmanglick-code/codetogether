const express = require('express');
const port = process.env.PORT || 3000;

var app = express();
var app = express();
app.use(express.static(__dirname + '/public')); //http://localhost:3000/Welcome.html

app.get('/', (req,res) => {
  res.send('<h1>Hello Express!</h1>');
})

app.get('/Andrew', (req, res) => {
  res.send({
    name: 'Andrew',
    likes: [
      'Biking',
      'Cities'
    ]
  });
});

app.get('/about', (req, res) => {
  res.send('About Page');
});


// /bad - send back json with errorMessage
app.get('/bad', (req, res) => {
  res.send({
    errorMessage: 'Unable to handle request'
  });
});

// app.listen(3000);
app.listen(port,() =>{
   console.log('Server is running on port:', port);
});
