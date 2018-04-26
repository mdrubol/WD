
app.controller("dataTableController", ["$scope", "tableFactory", function ($scope, tableFactory) {
    /*root Items start*/
    rootScope.title = "WD-DataTable";
    rootScope.dataset = "";
    rootScope.datatable = "active";
    rootScope.executeNonQuery = "";
    rootScope.executeScalar = "";
    rootScope.executeFilter = "";
    rootScope.executeEncrypt = "";
    rootScope.executeFaq = "";
    /*root Items end*/
    $scope.authenticationKey = "UMeiS+n70CAAHQNA7TiW2g==";
    $scope.dbProviders = [
                                 { text: "Sql", value: "1" },
                                 { text: "Db2", value: "2" },
                                 { text: "Oracle", value: "4" }
    ];
    $scope.dbProvider = { text: "Sql", value: "1" };
    $scope.commandTypes = [
                             { text: "Text", value: "1" },
                             { text: "StoredProcedure", value: "4" }
                           
                          ];
    $scope.commandType = { text: "Text", value: "1" };
    $scope.commandText = "SELECT * FROM TempEmployee";

    $scope.conStrings = [
                        { ConnectionName: "customized", ConnectionString: "server=172.21.42.128;database=XMMS;uid=xmms_user;password=xmms_user*123" },
                        { ConnectionName: "", ConnectionString: "" }
    ];
    $scope.conString = { ConnectionName: "", ConnectionString: "" };
    $scope.data = [];
    $scope.Params = [];
    $scope.hider = false;
    $scope.getDataTable = function () {
        $scope.status = "";
        var aParams = [];
        for (var p = 0; p < $scope.Params.length; p++) {

            aParams.push({ ParameterName: $scope.Params[p].ParameterName, ParameterValue: $scope.Params[p].ParameterValue });

        }
        var input = {
            "AuthenticationToken": $scope.authenticationKey,
            "TheSql": [{ commandText: $scope.commandText, commandType: $scope.commandType.value, Params: aParams }],
            "Connect": {
                "DbProvider": $scope.dbProvider.value,
                "ConnectionString": $scope.conString.ConnectionString
            }
        };
        tableFactory.getDataTable(JSON.stringify(input))
            .then(function (response) {
                $scope.status = "";
                $scope.data = response.data;
            }, function (error) {
                $scope.status = 'Unable to load customer data: ' + JSON.stringify(error.data);
            });
    };
    $scope.addParam = function () {
        $scope.Params.push({ ParameterName: "", ParameterValue: "" });
    }
    $scope.deleteParam = function (p) {
        $scope.Params.pop(p);
    }
    $scope.getProvider = function () {
        if ($scope.dbProvider.value == "1") {
            $scope.hider = false;
        }
        else {
            $scope.conString = { ConnectionName: "", ConnectionString: "" };
            $scope.hider = true;
        }
    };
}]);