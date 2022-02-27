const express = require('express');
const hbs = require('hbs');
const fs = require('fs');

// Added Git

var app = express();
app.use(express.static(__dirname + '/public')); //http://localhost:3000/Welcome.html

app.set('view engine', 'hbs');
hbs.registerPartials(__dirname + '/views/partials');
// -----------------------------------------------------
app.use((req, res, next) => {
  var now = new Date().toString();
  var log = `${now}: ${req.method} ${req.url}`;
  console.log(log);
  fs.appendFile('server.log', log + '\n', (err) =>{
    if(err){
        console.log('Cannot write to file');
    }
  });
  next(); // Allows to move on
});

fs.appendFile('MyGreeting.txt', 'Writing Successful!!!',function(err){
  if(err)
  {
    console.log('Cannot write to file');
  }
});

// Below will work like Site Maintenance Page
// app.use((req, res, next) => {
//     res.render('maintenance.hbs');
// });

// -----------------------------------------------------
hbs.registerHelper('getCurrentYear', () => {
  return new Date().getFullYear();
});

hbs.registerHelper('yourMessage', (text) => {
  return text.toUpperCase();
});
// -----------------------------------------------------
app.get('/', (req, res) => {
  res.render('welcome.hbs', {
    pageTitle: 'Welcome Page thru HandleBars - Advanced Templates '
  })
});
// -----------------------------------------------------
app.listen(5000,() =>{
   console.log('Server is running on port 5000');
});
