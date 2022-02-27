console.log('Loading function');

exports.handler = async (event, context) => {
    console.log(JSON.stringify(event, null, 2));
    var payload;
    event.Records.forEach(record => {
        // Kinesis data is base64 encoded so decode here
        payload = new Buffer(record.kinesis.data, 'base64').toString('ascii');
        console.log('Consumer#1 Decoded payload:', payload);
    });
    context.succeed();
    
};
