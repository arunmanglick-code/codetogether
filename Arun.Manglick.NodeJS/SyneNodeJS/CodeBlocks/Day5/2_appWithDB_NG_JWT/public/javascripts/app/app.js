var tApp = angular.module("tApp", ["ngRoute"]).config(
    function ($routeProvider) {
        $routeProvider.when("/ulist", {
            controller: "UserListController",
            templateUrl: "/userlist"
        });

        $routeProvider.when("/nuser", {
            controller: "NewUserController",
            templateUrl: "/newuser"
        });

        $routeProvider.otherwise({
            redirectTo: "/ulist"
        });
    });