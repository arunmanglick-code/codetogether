const http = require("http");
const axios = require('axios');

// function LoadME () {  
//     console.log("Load Test Started at port 3000....");
//     let res = http.get("http://127.0.0.1:58437/");
//     // let j = JSON.parse(res.body);
//     console.log("Load Test Result", res.data);
// }

axios.get('http://127.0.0.1:58437')
  .then(response => {
    console.log(response.data);
  })
  .catch(error => {
    console.log(error);
  });

// LoadME ();