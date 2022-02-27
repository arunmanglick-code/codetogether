const monk = require('monk');
const db = monk("localhost/nodetest1");
const usr_collection = db.get("usercollection");
const cre_collection = db.get("credentialcollection");
// -------------------------------------------------
module.exports.getUserCredentials = function (uname) {
    return new Promise((resolve,reject) => {
        cre_collection.findOne({"username":uname}).then(result => {
            resolve(result);
        }, err => {
            console.log(err);
            reject("Some Error, Contact DAL Dev.");
        });
    });
};
// -------------------------------------------------
module.exports.getAllUsersWithP = function () {
    return new Promise((resolve,reject) => {
        usr_collection.find({}).then(results => {
            resolve(results);
        }, err => {
            console.log(err);
            reject("Some Error, Contact DAL Dev.");
        });
    });
};
// -------------------------------------------------
module.exports.insertUser = function (data) {
    return new Promise((resolve,reject) => {
        usr_collection.insert(data).then(result => {
            resolve(result);
        }, err => {
            console.log(err);
            reject("Some Error, Contact DAL Dev.");
        });
    });
};
// -------------------------------------------------