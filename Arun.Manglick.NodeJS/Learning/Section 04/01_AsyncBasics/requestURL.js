// CMD: node requestURL.js

const request = require('request');
var myUrl = 'https://maps.googleapis.com/maps/api/geocode/json?address=1301%20lombard%20street%20philadelphia';
request(myUrl, function (error, response, body) {
  if (!error && response.statusCode == 200) {
    console.log(body) // Show the HTML for the Google homepage.
  }
  else {
    console.log(error)
  }
})
//--------------------------------------------------------
// request({
//   url: 'https://maps.googleapis.com/maps/api/geocode/json?address=1301%20lombard%20street%20philadelphia',
//   json: false
// }, (error, response, body) => {
//   console.log(body);
// });
//--------------------------------------------------------
// request({
//   url: 'https://maps.googleapis.com/maps/api/geocode/json?address=1301%20lombard%20street%20philadelphia',
//   json: true
// }, (error, response, body) => {
//   console.log(JSON.stringify(error, undefined, 2));
// });
//--------------------------------------------------------
// request({
//   url: 'https://maps.googleapis.com/maps/api/geocode/json?address=1301%20lombard%20street%20philadelphia',
//   json: true
// }, (error, response, body) => {
//     console.log(`Address: ${body.results[0].formatted_address}`);
//     console.log(`Latitude: ${body.results[0].geometry.location.lat}`);
//     console.log(`Longitude: ${body.results[0].geometry.location.lng}`);
// });
