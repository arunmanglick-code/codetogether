const EventEmitter = require('events');

class StringEmitter extends EventEmitter {
    constructor() {
        super();
        this.run();
    }

    run() {
        var self = this;
        setInterval(function () {
            var str = (function () {
                const stringsArr = ['NodeJs', 'AngularJs', 'JavaScript', 'ReactJs'];
                return stringsArr[Math.floor(Math.random() * stringsArr.length)];
            })();
            self.emit('data', str);
        }, 3000);
    }
};

module.exports = StringEmitter;