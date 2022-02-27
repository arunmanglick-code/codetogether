process.env.NODE_ENV = 'test';

const chai = require('chai');
const chaiHttp = require('chai-http');  // require('supertest');
const should = chai.should(); // require('expect');
const app = require('../app');

chai.use(chaiHttp)
// --------------------------------------------------------------------
// describe('Express Test Cases', ()=>{
//     describe('/GET users',()=>{
//         it('it should GET all the users', (done)=>{
//             chai.request(app)
//             .get('/api/users')
//             .end((err,res)=>{
//                 res.should.have.status(200);
//                 res.body.should.be.a('array');
//                 // res.body.length.should.be.eql(12);   
//                 res.body.length.should.not.equal(0);             
//                 done();
//             });
//         });
//     });

//     describe('/POST users',()=>{
//         it('it should insert a User', (done)=>{
//             let user = {
//                 username: "Test1",
//                 email: "test1@test.com"
//             };
//             chai.request(app)
//             .post('/api/users')
//             .send(user).end((err,res)=>{
//                 res.should.have.status(200);
//                 res.body.should.be.a('object');
//                 done();
//             });
//         });
//     });
// });
// --------------------------------------------------------------------