var ebs = require("./urls.js");
const apiURL = ebs.apiURL;
var sess;

exports.list = function (req, res, next) {
  res.render('lookup', { title: 'Lookup Tag'});
};

exports.testTagFound = function (req, res, next) {
  res.render('lookup', { title: 'Lookup Tag', tag: req.params.tag, tagFound: req.params.tagFound });
};

exports.testTagFulfilled = function (req, res, next) {
  res.render('lookup', { title: 'Lookup Tag', tag: req.params.tag, fulfilled: req.params.fulfilled });
};

exports.Submit = function (req, res, next) {

  var http = require('http');
  sess = req.session;
  var tagVerificationDetails = "";
  var logLookupDetails = "";
  var fulfillmentDetails = "";
  var tagID = '';
  var testTagVerificationAPI = {
      hostname: apiURL,
      //port: 8080,
      path: '/lookUpTags/'+ req.body.TAG,
      method: 'GET',
      headers: {
          'Content-Type': 'application/json',
        }
  };
  var testLogLookupAPI = {
      hostname: apiURL,
      //port: 8080,
      path: '/logLookup',
      method: 'POST',
      headers: {
          'Content-Type': 'application/json',
      }
  };
  var testFulfillmentAPI = {
      hostname: apiURL,
      //port: 8080,
      path: '/initiateClaimFulFillment',
      method: 'POST',
      headers: {
          'Content-Type': 'application/json'
      }
  };

  if (req.body.FNAME !== undefined) {
    var logLookupRequest = http.request(testLogLookupAPI, function (resLogLookup) {
      resLogLookup.setEncoding('utf8');
      resLogLookup.on('data', function (resLogLookup) {
        logLookupDetails += resLogLookup;
      });
      resLogLookup.on('end', function () {
        //console.log('lookupdetails =>'+logLookupDetails);
        logLookupDetails = JSON.parse(logLookupDetails);

        if (logLookupDetails.result) {
          var fulfillmentRequest = http.request(testFulfillmentAPI, function (resFulfillment) {
            resFulfillment.on('data', function (resFulfillment) {
              fulfillmentDetails += resFulfillment;
            });

            resFulfillment.on('end', function () {
              fulfillmentDetails = JSON.parse(fulfillmentDetails);
              //console.log(fulfillmentDetails);
              if (fulfillmentDetails.MessageId !== undefined) {
                res.redirect('/lookup/tag/'+req.body.TAG+'/fulfilled/true');
              }
              //console.log('Final =>'+fulfillmentDetails);
            });
          });

          var pushFulfillment = {
              "GJ_FirstName": req.body.FNAME,
              "GJ_LastName": req.body.LNAME,
              "GJ_Email": req.body.EMAIL,
              "GJ_Phone": req.body.PHONE,
              "GJ_Address": req.body.ADDRESS1,
              "GJ_LookUpDate": Date.now(),
              "Sub_FirstName": sess.tagInformation.FirstName,
              "Sub_LastName": sess.tagInformation.LastName,
              "Sub_Email": sess.tagInformation.Email,
              "MDN": sess.tagInformation.MDN,
              "TagName": sess.tagInformation.TagName,
              "TagEnrollId": sess.tagInformation.idTagEnroll
          };
          //console.log(JSON.stringify(pushFulfillment));
          fulfillmentRequest.write(JSON.stringify(pushFulfillment));
          fulfillmentRequest.end();
        }
      });
    });

    logLookupRequest.on('error', function(e) {
        console.error('ERROR '+e);
    });

    var goodJohn = {
        "FirstName": req.body.FNAME,
        "LastName": req.body.LNAME,
        "Email": req.body.EMAIL,
        "Phone": req.body.PHONE,
        "Address": req.body.ADDRESS1 + req.body.ADDRESS2,
        "TagName": sess.tagInformation.TagName,
        "TagEnrollId": sess.tagInformation.idTagEnroll,
        "TagFound": "Yes"
    };
    //console.log('GoodJohn =>'+JSON.stringify(goodJohn));
    logLookupRequest.write(JSON.stringify(goodJohn));
    logLookupRequest.end();

  }
  else {
    //console.log(testTagVerificationAPI);
    var reqGet = http.request(testTagVerificationAPI, function (resTagVerification) {
      resTagVerification.setEncoding('utf8');
      resTagVerification.on('data', function (resTag) {
        tagVerificationDetails += resTag;
      });

      resTagVerification.on('end', function () {
        tagVerificationDetails = JSON.parse(tagVerificationDetails);
        if (tagVerificationDetails.result !== undefined) {
          res.redirect('/lookup/tag/'+req.body.TAG+'/tagFound/false');
        }
        else {
          sess.tagInformation = {
            "idTagEnroll" : tagVerificationDetails.idTagEnroll,
            "MDN" : tagVerificationDetails.MDN,
            "FirstName" : tagVerificationDetails.FirstName,
            "LastName" : tagVerificationDetails.LastName,
            "Email" : tagVerificationDetails.Email,
            "TagName" : tagVerificationDetails.TagName,
            "TagDescription" : tagVerificationDetails.TagDescription,
            "Status" : tagVerificationDetails.Status,
            "CreatedDate" : tagVerificationDetails.CreatedDate,
            "ActivationDate" : tagVerificationDetails.ActivationDate,
            "ModifiedDate" : tagVerificationDetails.ModifiedDate
          }
          res.redirect('/lookup/tag/'+req.body.TAG+'/tagFound/true');
        }
      });
    });
    reqGet.on('error', function(e) {
        console.error('ERROR '+e);
    });
    reqGet.end();
  }
};


exports.renderLookup = function (req, res, next) {
    res.render('lookup', {
        title: 'Lookup Tag'
    });
};

exports.result = function (req, res, next) {
    res.render('lookup', {
        title: 'Lookup Tag',
        result: req.params.result
    });
};
