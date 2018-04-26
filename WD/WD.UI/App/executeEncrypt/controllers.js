
app.controller("executeEncryptController", ["$scope", "executeEncryptFactory", function ($scope, executeEncryptFactory) {
    /*root Items start*/
    rootScope.title = "WD-Encrypt";
    rootScope.dataset = "";
    rootScope.datatable = "";
    rootScope.executeNonQuery = "";
    rootScope.executeScalar = "";
    rootScope.executeFilter = "";
    rootScope.executeEncrypt = "active";
    rootScope.executeFaq = "";
    $scope.databaseName = "WDMBR1";
    $scope.serverName = "172.22.24.181";
    $scope.userID = "performa";
    $scope.password = "performa";
    $scope.encrypt = function () {
        var conString = "Database=" + $scope.databaseName + ";Server=" + $scope.serverName + ";User Id=" + $scope.userID + ";Password=" + $scope.password + ";Connect Timeout=30;";
        executeEncryptFactory.encrypt(conString)
           .then(function (response) {
               $scope.status = response.data;
           }, function (error) {
               $scope.status = 'Unable to load customer data: ' + JSON.stringify(error.data);
           });
        };
      
    }]);