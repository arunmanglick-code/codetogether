var tApp = angular.module("tApp", ["ngRoute"]).config(
    function ($routeProvider) {
        $routeProvider.when("/ulist", {
            controller: "UserListController",// This will get users list which will then be shown by views/userlist.jade
            templateUrl: "/userlist" // This will go to node.js routes - routes/index.js which then takes to views/userlist.jade
        });

        $routeProvider.when("/nuser", {
            templateUrl: "/newuser"
        });

        $routeProvider.otherwise({
            redirectTo: "/ulist"
        });
    });