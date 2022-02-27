//CMD: node requestEncodeURL.js -a "1301 lombard street philadelphia"

const request = require('request');
const yargs = require('yargs');
const geocode = require('./requestAddress');

const addressOptions = {
  describe: 'Weather Address',
  demand: true,
  alias: 'address',
  string:true
};

const argv = yargs
  .options({a:addressOptions})
  .argv;
//--------------------------------------------------------
geocode.geocodeAddress(argv.address, function (message) {
  console.log(message);
});


var callMeBack = function (errorMessage, results) {
  if (errorMessage) {
    console.log(errorMessage);
  } else {
    console.log(JSON.stringify(results, undefined, 2));
  }
};
