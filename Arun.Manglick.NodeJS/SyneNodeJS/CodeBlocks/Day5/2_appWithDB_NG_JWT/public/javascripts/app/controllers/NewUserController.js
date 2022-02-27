// Request will come here - NewUserController 
// Here user will first get authenticate (\2_appWithDB_NG_JWT\routes\api.js) and on success token will be returned to above controller
// Controller will then store this token in window’s session storage (can be stored in cookie too)
// After on every request this token will be first retrieved from Windows session and sent back with every request


tApp.controller("NewUserController", function ($scope, $http, $location,$window) {
    $scope.SaveUser = function (user) {
        var token = $window.sessionStorage.getItem("s_token");
        $http.post("/api/users", user, {
            headers: {
                'x-access-token': token
            }
        }).then(function (response) {
            $location.path('/ulist');
        }, function () {
            alert("Error...");
        });
    };
});