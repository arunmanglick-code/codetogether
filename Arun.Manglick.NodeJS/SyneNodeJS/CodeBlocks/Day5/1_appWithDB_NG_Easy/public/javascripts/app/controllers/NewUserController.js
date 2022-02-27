tApp.controller("NewUserController", function ($scope,$http,$location) {  
    $scope.SaveUser = function (user) {  
        $http.post("/api/users", user).then(function(response){
            $location.path('/ulist');
        }, function () {  
            alert("Error...");
        });
    };
});