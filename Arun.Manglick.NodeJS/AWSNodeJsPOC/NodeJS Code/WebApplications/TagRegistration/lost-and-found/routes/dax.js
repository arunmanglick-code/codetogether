var ebs = require("./urls.js");
const apiURL = ebs.apiURL;

exports.Submit = function (req, res, next) {
    console.log('Dax Submit click');

    var http = require('http');

    //Variable declarations and control binding
    var tagName = req.body.TAG;
    var apiResult;
    //DUMMY API
    var fulfillmentAPI = {
        hostname: apiURL,
        path: '/completeClaimFulFillment/' + tagName,
        method: 'POST'
        //headers: {
        //    'Content-Type': 'application/json'
        //}
    };
    //DUMMY API

    var reqGet = http.request(fulfillmentAPI, function (resFulfillment) {
        resFulfillment.on('data', function (resTag) {
            console.info('GOT response of fulfillmentAPI as:\n' + resTag);
            apiResult = resTag;
        });
        resFulfillment.on('end', function () {
            var obj = JSON.parse(apiResult);
            if (obj.result) {
                res.redirect('/dax/tag/' + req.body.TAG + '/fulfilled/true');
            }
            else
            {
                res.redirect('/dax/tag/' + req.body.TAG + '/fulfilled/false');
            }
        });
    });
    reqGet.write(tagName);
    reqGet.on('error', function (e) {
        console.error(e);
    });
    reqGet.end();
}


exports.renderDax = function (req, res, next) {
    res.render('dax', {
        title: 'DAX'
    });
};

exports.result = function (req, res, next) {
    res.render('dax', {
        title: 'DAX',
        result: req.params.result
    });
};

exports.testTagFulfilled = function (req, res, next) {
    res.render('dax', { title: 'DAX', tag: req.params.tag, fulfilled: req.params.fulfilled });
};
