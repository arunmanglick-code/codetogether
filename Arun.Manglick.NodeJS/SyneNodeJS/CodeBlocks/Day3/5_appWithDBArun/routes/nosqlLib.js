module.exports.NoSQLFactory = function () {
    this.db = function (msg) {
        console.log(msg + ", Connected to Mongo database");

        var monk = require('monk');
        var db = monk("localhost/nodetest1");
        var collection = db.get("usercollection");
        return collection;
    };

    // this.add = function (msg) {
    //     console.log(msg + ", Connected to Mongo database");
    //
    //     var monk = require('monk');
    //     var db = monk("localhost/nodetest1");
    //     var collection = db.get("usercollection");
    //
    //     return collection;
    // };
};
