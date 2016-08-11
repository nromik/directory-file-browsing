var explorerModel = angular.module('explorerModel', []);

console.log(" model");
explorerModel.controller('explorerCtrl', function ($scope, $http) {
    $scope.count = 0;
    $scope.isCountFiles = true;
    console.log(" constructor");
    $scope.loading = true;
    
    $scope.Transit = function (path) {

        $scope.loading = false;
        $scope.directoreis = null;
        $scope.files = null;
        $scope.errorMessage =null;
        $scope.parentDirectory = null;
        $scope.currentDirectory = null;
        $scope.countFiles = null;

        console.log(" $scope.Transit (" + path + ")");




        $http.get("api/DirectoryInfo?path=" + path
                + "&isCountFiles=" + $scope.isCountFiles).then(function (response) {
            $scope.count++;
            $scope.myData = response.data;
            $scope.directoreis = response.data.directoreis;
            $scope.files = response.data.files;
            $scope.errorMessage = response.data.errorMessage;
            $scope.parentDirectory = response.data.parentDirectory;
            $scope.currentDirectory = response.data.currentDirectory;
            $scope.countFiles = response.data.countFiles;
            $scope.loading = true;
        });

       
    }

    $scope.ReloadData = function() {
        $scope.Transit($scope.currentDirectory);
    }

    $scope.Transit("");
});

console.log(" model");