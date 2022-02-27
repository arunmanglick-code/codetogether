var express = require('express');
var routes = require('./routes');
var tasks = require('./routes/tasks');
var lookup = require('./routes/lookup');
var dax = require('./routes/dax');
var connect = require('./routes/connect');

var http = require('http');
var path = require('path');
var app = express();

var favicon = require('serve-favicon'),
  logger = require('morgan'),
  bodyParser = require('body-parser'),
  methodOverride = require('method-override'),
  cookieParser = require('cookie-parser'),
  session = require('express-session'),
  errorHandler = require('errorhandler');

app.locals.compname = 'Asurion'
app.locals.appname = 'Asurion - L & F Tag utility'

app.set('port', process.env.PORT || 3000);
app.set('views', __dirname + '/views');
app.set('view engine', 'jade');
app.use(favicon(path.join('public','favicon.ico')));
//app.use(logger('dev'));
app.use(session({secret: 'L&F'}));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended: true}));
app.use(methodOverride());
app.use(require('less-middleware')(path.join(__dirname, 'public')));
app.use(express.static(path.join(__dirname, 'public')));
// -------------------------------------------------------------
app.get('/', routes.index);
// -------------------------------------------------------------
app.get('/checkAPIConnect', connect.test);
app.get('/checkAPIConnect/connect/:connect', connect.result);
// -------------------------------------------------------------
app.get('/tasks', tasks.list);
app.post('/tasks', tasks.Submit);
app.get('/tasks/mdn/:mdn/result/:result', tasks.result);

// -------------------------------------------------------------
app.get('/lookup', lookup.list);
app.post('/lookup', lookup.Submit);
app.get('/lookup/tag/:tag/tagFound/:tagFound/', lookup.testTagFound);
app.get('/lookup/tag/:tag/fulfilled/:fulfilled/', lookup.testTagFulfilled);

// -------------------------------------------------------------
app.get('/dax/result/:result/', dax.result);
app.post('/dax', dax.Submit);
app.get('/dax', dax.renderDax);
app.get('/dax/tag/:tag/fulfilled/:fulfilled/', dax.testTagFulfilled);

// -------------------------------------------------------------
app.all('*', function(req, res){
  res.status(404).send();
})

http.createServer(app).listen(app.get('port'), function(){
  console.log('Express server listening on port ' + app.get('port'));
});
