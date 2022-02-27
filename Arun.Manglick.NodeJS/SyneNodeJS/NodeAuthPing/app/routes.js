module.exports = function(app, passport) {

    // route for home page
    app.get('/', function(req, res) {
        res.render('pages/index.ejs'); // load the index.ejs file
    });

    // route for showing the profile page
    app.get('/profile', isLoggedIn, function(req, res) {
        res.render('pages/profile.ejs', {
            user: req.user,
            flash: req.session.flash
        });
    });

    // route for logging out
    app.get('/logout', function(req, res) {
        req.logout();
        res.redirect('/');
    });

    // route for signing in
    app.get('/auth/oauth2', passport.authenticate('oauth2', {
        scope: ['openid', 'profile', 'email']
    }));

    // route for call back

    app.get('/auth/oauth2/callback',
        passport.authenticate('oauth2', {
            failureRedirect: '/login'
        }),
        function(req, res) {
            res.redirect('/profile');
//		console.log(req);
        });

};

// route middleware to make sure a user is logged in
function isLoggedIn(req, res, next) {

    // if user is authenticated in the session, carry on
    if (req.isAuthenticated())
        return next();

    // if they aren't redirect them to the home page
    res.redirect('/');
}
