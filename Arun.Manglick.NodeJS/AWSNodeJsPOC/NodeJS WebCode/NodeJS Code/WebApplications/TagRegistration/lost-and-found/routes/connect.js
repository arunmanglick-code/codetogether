var ebs = require("./urls.js");
const apiURL = ebs.apiURL;

exports.test = function(req, res, next){
	  var http = require('http');
	  sess = req.session;
	  var connectionResult = "";
	  var checkAPIConnect = {
	      hostname: apiURL,
	      //port: process.env.PORT || 8081,
	      path: '/checkAPIConnection',
	      method: 'GET',
	      headers: {
	          'Content-Type': 'application/json',
	        }
	  };

	var reqGet = http.request(checkAPIConnect, function (resAPIConnect) {
      resAPIConnect.setEncoding('utf8');

      resAPIConnect.on('data', function (res) {
        connectionResult += res;
      });

      resAPIConnect.on('end', function () {
        connectionResult = JSON.parse(connectionResult);
        if (connectionResult.connect !== undefined && connectionResult.connect == true) {
        	console.log(connectionResult.connect);
          res.redirect('/checkAPIConnect/connect/true');
        }
        else {
          res.redirect('/checkAPIConnect/connect/false');
        }
      });
    });

	reqGet.on('error', function(e) {
	    console.error('ERROR '+e);
	});
	reqGet.end();
};

exports.result = function(req, res, next){
  res.render('connect', {
    title: 'Test API Connection',
    connect: req.params.connect
  });
};
