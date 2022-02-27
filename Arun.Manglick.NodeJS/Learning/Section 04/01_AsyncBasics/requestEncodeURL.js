//CMD: node requestEncodeURL.js -a "1301 lombard street philadelphia"

const request = require('request');
const yargs = require('yargs');

const addressOptions = {
  describe: 'Weather Address',
  demand: true,
  alias: 'address',
  string:true
};

const argv = yargs
  .options({a:addressOptions})
  .argv;

console.log('Argv is:' , argv);

var encodedAddress = encodeURIComponent(argv.address);
console.log('Encoded Address is:' , encodedAddress);

request({
  url: `https://maps.googleapis.com/maps/api/geocode/json?address=${encodedAddress}`,
  json: true
}, (error, response, body) => {
  if (error) {
    console.log('Unable to connect to Google servers.');
  } else if (body.status === 'ZERO_RESULTS') {
    console.log('Unable to find that address.');
  } else if (body.status === 'OK') {
    console.log(`Address: ${body.results[0].formatted_address}`);
    console.log(`Latitude: ${body.results[0].geometry.location.lat}`);
    console.log(`Longitude: ${body.results[0].geometry.location.lng}`);
  }
});
