const sql = require('mssql');
const AWS = require('aws-sdk');

let awsSdk = new AWS.SecretsManager();

exports.handler = async (event, context) => {
  console.log('START!!!!!!!');

  const [userName, password] = await Promise.all([
    GetSecretValue('AM/Dev/Store/DW/Username'),
    GetSecretValue('AM/Dev/Store/DW/Password')
  ]).catch((err) => {
    console.log('Error AT getting secrets....', err);
    throw err;
  });

  
  console.log('userName:', userName);
  console.log('password:', password);

  const [host, database] = [process.env.host, process.env.database];
  const sp_Name= 'sp_LookUpTagEnrollment';

  console.log('userName:', userName);
  console.log('password:', password);
  console.log('host:', host);
  console.log('database:', database);
  console.log('Sp:', sp_Name);

  const sqlConfig = {
    user: userName,
    password: password,
    database: database,
    server: host,
    pool: {
      max: 10,
      min: 0,
      idleTimeoutMillis: 30000
    },
    options: {
      trustServerCertificate: true
    }
  }

  await get();

  async function get() {
    try {
      console.log('Connecting.....');
      let pool = await sql.connect(sqlConfig)
      console.log('Connected!!!!!!!');

      let query = 'select * from customer';
      //let query = 'CALL ' + database + '.' + sp_LookUpTagEnrollment + '("' + tagName + '");'
      let result = await pool.request().query(query);
      // console.log('result:', result);
    } catch (err) {
      console.log('ERROR:', err);
    }
  }

  async function GetSecretValue(secretKey) {
    try {
      const secretData = await awsSdk.getSecretValue({
        SecretId: secretKey
      }).promise().catch((err) => {
        return err;
      });

      if (secretData.SecretString) {
        return (secretData.SecretString);
      } else {
        throw new Error('Secret Not Found!');
      }
    } catch (err) {
      return err;
    }
  }
}