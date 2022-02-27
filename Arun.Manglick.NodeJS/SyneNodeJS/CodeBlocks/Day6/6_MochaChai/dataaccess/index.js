const config = require('config');
const monk = require('monk');
const db = monk(config.DBHost);
const collection = db.get("usercollection");

module.exports.getAllUsers = function () {
    return new Promise((resolve,reject) => {
        collection.find({}).then(results => {
            resolve(results);
        }, err => {
            console.log(err);
            reject("Some Error, Contact DAL Dev.");
        });
    });
};

module.exports.insertUser = function (data) {
    return new Promise((resolve,reject) => {
        collection.insert(data).then(result => {
            resolve(result);
        }, err => {
            console.log(err);
            reject("Some Error, Contact DAL Dev.");
        });
    });
};