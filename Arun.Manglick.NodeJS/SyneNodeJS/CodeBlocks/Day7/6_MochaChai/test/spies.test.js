process.env.NODE_ENV = 'test';

const chai = require('chai');
const sinon = require('sinon');
const sinonChai = require('chai-sinon');
const app = require('../app');
const should = chai.should();
chai.use(sinonChai);

// ---------------------------------------------------------
function hello(cb){
    //Code to Process output
    console.log('Hello Called Once');
    cb("hello");
}

// describe('Test Hello Callback', ()=>{
//     it("should call callback once with hello",function(){
//         var callback = sinon.spy();
//         hello(callback);

//         callback.should.have.been.calledOnce;
//         callback.should.have.been.calledWith("hello");
//     });
// });
// ---------------------------------------------------------
// function hello(cb){
//     //Code to Process output
//     // cb("hello");
// }

// describe('hello', ()=>{
//     it("should call callback once",function(){
//         var callback = sinon.spy();
//         hello(callback);
//         callback.should.have.been.calledOnce;
//     });

//     it("should call callback with hello",function(){
//         var callback = sinon.spy();
//         hello(callback);
//         callback.should.have.been.calledWith("hello");
//     });
// });
// ---------------------------------------------------------
// var da = {
//     setName: function (name) {
//         console.log("Set Name Called....");
//         this.name = name;
//     }
// }

// var setNameSpy = sinon.spy(da, 'setName');

// da.setName("Synechron");
// da.setName("Synechron");
// da.setName("Synechron");
// console.log(setNameSpy.callCount);

// setNameSpy.restore();
// ---------------------------------------------------------