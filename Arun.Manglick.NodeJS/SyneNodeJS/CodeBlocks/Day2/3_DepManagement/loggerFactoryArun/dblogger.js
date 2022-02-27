module.exports.DBLogger = function () {
    this.log = function (msg) {
        console.log(msg + ", logged on database.");
    };
};