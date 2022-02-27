tApp.controller("UserListController", function ($scope,$http) {  

    // Node.js API
    // $http.get("/api/users").then(function(response){
    //     $scope.users = response.data;
    //     console.log($scope.users);
    // });

    let users = [
        {id:1,username:"Ram"},
        {id:2,username:"RK"},
        {id:3,username:"Manish"},
        {id:4,username:"Abhijeet"},
        {id:5,username:"Varun"}
    ];

    $scope.users = users;
    console.log($scope.users);
});