const express = require('express');
const hbs = require('hbs');

var app = express();
app.set('view engine', 'hbs');
app.use(express.static(__dirname + '/public')); //http://localhost:3000/Welcome.html

app.get('/', (req, res) => {
  res.render('welcome.hbs', {
    pageTitle: 'Welcome Page thru HandleBars ',
    currentYear: new Date().getFullYear()
  })
});

app.get('/home', (req,res) => {
  res.render('home.hbs', {
    pageTitle: 'Home Page thru HandleBars',
    welcomeMessage: 'Welcome to my Node JS WebPage',
    currentYear: new Date().getFullYear()
  });
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

// /bad - send back json with errorMessage
app.get('/bad', (req, res) => {
  res.send({
    errorMessage: 'Unable to handle request'
  });
});

app.listen(3000);
app.listen(4000,() =>{
   console.log('Server is running on port 4000');
});
