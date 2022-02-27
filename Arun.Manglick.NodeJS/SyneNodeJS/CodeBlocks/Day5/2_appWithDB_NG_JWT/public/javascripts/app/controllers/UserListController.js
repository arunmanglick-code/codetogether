// Request will come here - UserListController 
// Here user will first get authenticate (\2_appWithDB_NG_JWT\routes\api.js) and on success token will be returned to above controller
// Controller will then store this token in window’s session storage (can be stored in cookie too)
// After on every request this token will be first retrieved from Windows session and sent back with every request

tApp.controller("UserListController", function ($scope, $http, $window) {
    $http.post('/api/authenticate', {
        username: "manish",
        password: "manish"
    }).then(function (response) {
        console.log(response.data);
        $window.sessionStorage.setItem("s_token", response.data.token);
    });

    $scope.getData = function () {
        var token = $window.sessionStorage.getItem("s_token");
        $http.get("/api/users", {
            headers: {
                'x-access-token': token
            }
        }).then(function (response) {
            $scope.users = response.data;
            console.log($scope.users);
        });
    }
});


