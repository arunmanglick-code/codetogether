process.env.NODE_ENV = 'test';

const chai = require('chai');
const sinon = require('sinon');
const sinonChai = require('chai-sinon');
const app = require('../app');
const should = chai.should();
chai.use(sinonChai);

//---------------------------------------------Stubs
var da = {
    getName: function (id, cb) {
        console.log("Get Name Called, getting from Database....");        
        var data = {}; //Call to DB
        cb(data);
    }
}

describe('setUserName', function () {
    it('should call callback after getname', function () {
        // var gSpy = sinon.spy(da, 'getName');
        var stub = sinon.stub(da, 'getName');
        stub.yields();

        var callback = sinon.spy();
        da.getName(1, callback);

        stub.restore();            
        console.log(stub.callCount);
        callback.should.have.been.calledOnce;
    })
});
//-------------------------------------------
