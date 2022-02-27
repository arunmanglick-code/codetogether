var http = require('http');
var fs = require('fs');

var server = http.createServer(function (req, res) {
    fs.readFile('./index.html', function (err, html) {
        if (err) throw err;

        res.writeHeader(200, {
            "Content-Type": "text/html"
        });
        res.write(html);
        res.end();
    });
});

server.listen("3000", function () {
    console.log("Server Started....");
});

var WebSocketServer = require('websocket').server;

wsServer = new WebSocketServer({
    httpServer: server
});

var count = 0;
var clients = {};

wsServer.on('request', function (r) {
    var connection = r.accept('echo-protocol', r.origin);

    var id = count++;

    clients[id] = connection;
    console.log('Connection Accepted [' + id + ']');

    connection.on('message', function (message) {
        var msg = message.utf8Data;

        for (var i in clients) {
            clients[i].sendUTF("From Server - " + msg);
        }
    });

    connection.on('close', function (message) {
        delete clients[id];
        console.log('Connection Closed [' + id + ']');
    });
});