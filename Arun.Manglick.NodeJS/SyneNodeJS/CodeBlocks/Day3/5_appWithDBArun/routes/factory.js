// Arun Way:
// module.exports.RDSFactory = function (msg) {
//   var rdsConnect = require('./rdsLib.js');
//   return new rdsConnect.RDSFactory();
// }
//
// module.exports.NoSQLFactory = function (msg) {
//   var nosqlConnect = require('./nosqlLib.js');
//   return new nosqlConnect.NoSQLFactory();
// }
// ------------------------------------------------
// Manish Way:

module.exports.NoSQLFactory = {
    getDB: function () {
      var nosqlConnect = require('./nosqlLib.js');
      return new nosqlConnect.NoSQLFactory();
    }
}

module.exports.RDSFactory = {
    getDB: function () {
      var rdsConnect = require('./rdsLib.js');
      return new rdsConnect.RDSFactory();
    }
}
//------------------------------------------------
