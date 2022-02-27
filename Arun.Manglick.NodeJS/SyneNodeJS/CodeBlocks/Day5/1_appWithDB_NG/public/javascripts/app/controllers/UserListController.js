tApp.controller("UserListController", function ($scope,$http) {  
    $http.get("/api/users").then(function(response){
        $scope.users = response.data;
        console.log($scope.users);
    });
});