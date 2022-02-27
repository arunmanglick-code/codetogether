process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

// require('ssl-root-cas').inject();
var passport = require('passport');
var OAuth2Strategy = require('passport-ping-oauth2').Strategy;
var ClientJWTBearerStrategy = require('passport-oauth2-jwt-bearer').Strategy;
//var OAuth2Strategy = require('passport-openid').Strategy;

var User = require('../app/models/user');
var configAuth = require('./auth');

module.exports = function(passport) {


    // used to serialize the user for the session
    passport.serializeUser(function(user, done) {
        done(null, user.id);
    });


    // used to deserialize the user
    passport.deserializeUser(function(id, done) {
        User.findById(id, function(err, user) {
            done(err, user);
        });
    });


    passport.use(new OAuth2Strategy({
            authorizationURL: configAuth.pingAuth.authorizationURL,
            tokenURL: configAuth.pingAuth.tokenURL,
            clientID: configAuth.pingAuth.clientID,
            clientSecret: configAuth.pingAuth.clientSecret,
            callbackURL: configAuth.pingAuth.callbackURL
        },
        function(accessToken, refreshToken, params, profile, done) {

            User.findOne({ 'ping.id': profile.cn }, function(err, user) {
                if (err) { 
                    return done(err);
                }
                if (user) {
                    return done(null, user);
                } else {
                    var newUser = new User();
                    newUser.ping.id    = profile.id;
//                    newUser.ping.idtoken    = idToken;
                    newUser.ping.token = accessToken;
                    newUser.ping.name  = profile.displayName;
                    newUser.ping.email = profile.email;
                    newUser.ping.entitlements = profile.entitlements;
//                    console.log('profile.id,accessToken,profile.displayName,profile.email,profile.photo');
//                    console.log(profile.id,accessToken,profile.displayName,profile.email,profile.photo);
                    newUser.save(function(err) {
                        if (err) { throw err; }
                            return done(null, newUser);
                    });
                }
            });
        }));
};
