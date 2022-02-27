module.exports.log = function (msg) {  
    var loggerComponent = require("./logger.js");
    var lgr = new loggerComponent.Logger();
    lgr.log(msg);
}