const AWS = require('aws-sdk');


AWS.config.update(
  {region: 'us-east-1'},
  {endpoint: 'email.us-east-1.amazonaws.com'}
);
//const ses = new AWS.SES({apiVersion: '2010-12-01'});
var ses = new AWS.SES({"accessKeyId":"AKIAIT347QFP7WNQ2HAQ", "secretAccessKey": "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", "region": "us-east-1"});

//Send Mail to registerd ids
var from = 'arun.manglick@asurion.com';
var to = ['arun.manglick@asurion.com'];
var FirstName = 'Arun';
var LastName = 'Manglick';
var Tag1 = 'TestTag11';
var Tag2 = 'TestTag12';
var Tag3 = 'TestTag13';
var link = 'http://custom-env-2.cmt9ppmsrk.us-east-1.elasticbeanstalk.com/tasks';

// Send Mail Function
ses.sendEmail( {
   Source: from,
   Destination: { ToAddresses: to },
   Message : {
       Subject : {
          Data: 'Tag Registration'
                 },
       Body: {
           Text: {
                    Data: `Hi ${FirstName} ${LastName}` + 
                    '\n\nWelcome to Asurion,'+
                    '\nPlease register tags by clicking on below link.' + 
                    `\n${link}` + 
                    `\n\nFirst Tag: ${Tag1}` + 
                    `\nSecond Tag: ${Tag2}` + 
                    `\nThird Tag: ${Tag3}` + 
                    '\n\nRegards,' + 
                    '\nTAG Team'
                 }
             }
           }
}
, function(err, data) {
    if(err)
    {
      console.log(err);
      //context.fail();
      return false;
    }
    else
    {
      console.log('Tag Registration Email sent Successfullly');
      console.log(data);
    }
 });
