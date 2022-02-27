module.exports.FLFactory = {
    getLogger: function () {
        var fComp = require('./filelogger.js');
        return new fComp.FileLogger();
    }
}

module.exports.DBLFactory = {
    getLogger: function () {
        var dComp = require('./dblogger.js');
        return new dComp.DBLogger();
    }
}