var tApp = angular.module("tApp", ["ngRoute"]).config(
    function ($routeProvider) {
        $routeProvider.when("/ulist", {
            templateUrl: "/userlist",
            controller: "UserListController"
        });

        $routeProvider.when("/nuser", {
            templateUrl: "/newuser",
            controller: "NewUserController"
        });

        $routeProvider.otherwise({
            redirectTo: "/ulist"
        });
    });