module.exports.FileLogger = function () {
    this.log = function (msg) {
        console.log(msg + ", logged on File.");
    };
};