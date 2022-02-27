process.env.NODE_ENV = 'test';

const chai = require('chai');
const sinon = require('sinon');
const sinonChai = require('chai-sinon');
const app = require('../app');
const should = chai.should();
chai.use(sinonChai);

//-----------------------------------------------------------Mocks
var da = {
    getName: function () {
        console.log("Get Name Called.......");
        return this.name;
    },
    setName: function (name) {
        console.log("Set Name Called......");
        this.name = name;
    }
}

function check(){
    da.setName("Manish");
    var data = da.getName();
    var data1 = da.getName();
}

// describe('setUserName', function () {
//     it('should set and get name atmost once', function () {
        
//         check();

//         var sMock = sinon.mock(da);
//         sMock.expects('setName').withArgs('Manish').atMost(1);
//         sMock.expects('getName').withArgs().atMost(1);        

//         sMock.restore();
//         sMock.verify();
//     })
// });
// ------------------------------------------------------