module.exports.RDSFactory = function () {
    this.db = function (msg) {
        console.log(msg + ", Connected to RDS database.");

        const sql = require('mssql');
        require("msnodesqlv8");

        let config = {
          server: 'ARUNMANGLICK-PC\\SQLEXPRESS', //'localhost\\SqlExpress',
          database: 'Northwind',
          driver: 'msnodesqlv8',
          options: {
            trustedConnection: true
          }
        };

        var connection = new sql.Connection(config);

        connection.connect().then((conn) => {
          var request = new sql.Request(conn);
          request.query('SELECT CustomerId,ContactName FROM Customers').then((results)=>{
            return results;
          },(err)=>{
              return err;
          });
        }, (reason) => {
          console.log(reason);
          return res;
        });

    };
};
