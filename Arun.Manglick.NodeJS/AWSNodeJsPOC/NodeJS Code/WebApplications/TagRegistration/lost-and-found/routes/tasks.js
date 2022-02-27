var ebs = require("./urls.js");
const apiURL = ebs.apiURL;

exports.lookup = function(req, res){
  res.render('lookup', { title: 'Lookup Tag' });
};

exports.list = function(req, res, next){
  res.render('tasks', {
    title: 'Register Tag'
  });
};

exports.result = function(req, res, next){
  res.render('tasks', {
    title: 'Register Tag',
    result: req.params.result
  });
};

exports.Submit = function(req, res, next){
  var message;

  var http = require("http");
  var options = {
    hostname: apiURL,
    //port: 8080,//process.env.PORT || 3000, // Port of nodeJS API
    path: '/registerTags', // Path(method name) of nodeJS API
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
    }
  };

  function tagDetail(tagName, tagStatus) {
    this.TagName = tagName;
    this.Status = tagStatus;
  }

  function tagRegisterRequest(mdn, tags) {
    this.MDN = mdn;
    this.TagsDetails = tags;
  }

  var tags = [new tagDetail(req.body.TAG1,'Active'), new tagDetail(req.body.TAG2, 'Active'), new tagDetail(req.body.TAG3, 'Active')];
  var myRequest = new tagRegisterRequest(req.body.MDN,tags)

  var reqAPI = http.request(options, function(resAPI) {
    resAPI.setEncoding('utf8');

    resAPI.on('data', function (body) {
      message = body;
      console.log(message);
    });

    resAPI.on('end', function () {
      message = JSON.parse(message);
      res.redirect('/tasks/mdn/'+req.body.MDN+'/result/'+message.result);
    });
  });

  reqAPI.on('error', function(e) {
    console.log('problem with request: ' + e.message);
  });

  reqAPI.write(JSON.stringify(myRequest));
  reqAPI.end();
};
