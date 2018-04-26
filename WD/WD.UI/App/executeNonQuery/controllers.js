
app.controller("executeNonQueryController", ["$scope", "executeNonQueryFactory", function ($scope, executeNonQueryFactory) {
    /*root Items start*/
    rootScope.title = "WD-ExecuteNonQuery";
    rootScope.dataSet = "";
    rootScope.datatable = "";
    rootScope.executeNonQuery = "active";
    rootScope.executeScalar = "";
    rootScope.executeEncrypt = "";
    rootScope.executeFilter = "";
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
    $scope.conStrings = [
                        { ConnectionName: "customized", ConnectionString: "server=172.21.42.128;database=XMMS;uid=xmms_user;password=xmms_user*123" },
                        { ConnectionName: "", ConnectionString: "" }
    ];
    $scope.conString = { ConnectionName: "", ConnectionString: "" };

   
    $scope.theSqls = [
                        { commandText: "INSERT INTO TempEmployee(EmployeeID,FirstName,LastName,MiddleName,DateOfJoining,Provider) VALUES(900,'INSERT','USING','API','10/10/2017','SQL')", commandType: { text: "Text", value: "1" }, Params: [] }
    ];
    $scope.addSql = function () {
        $scope.theSqls.push({ commandText: "", commandType: { text: "Text", value: "1" }, Params: [] });
        $scope.myCheck = ($scope.theSqls.length == 1) ? true : false;
    }
    $scope.deleteSql = function (x) {
        $scope.theSqls.pop(x);
        $scope.myCheck = ($scope.theSqls.length == 1) ? true : false;
    }
    $scope.myCheck = ($scope.theSqls.length == 1) ? true : false;
    $scope.hider = false;
    $scope.executeNonQuery = function () {
        $scope.status = "";
        var input = {
            "AuthenticationToken": $scope.authenticationKey,
            "TheSql": [],
            "Connect": {
                "DbProvider": $scope.dbProvider.value,
                "ConnectionString": $scope.conString.ConnectionString
            }
        };
        for (var i = 0; i < $scope.theSqls.length; i++) {
            var aParams = [];
            if ($scope.theSqls[i].commandText != '') {
                for (var p = 0; p < $scope.theSqls[i].Params.length; p++) {

                    aParams.push({ ParameterName: $scope.theSqls[i].Params[p].ParameterName, ParameterValue: $scope.theSqls[i].Params[p].ParameterValue });

                }
                input.TheSql.push({ commandText: $scope.theSqls[i].commandText, commandType: $scope.theSqls[i].commandType.value, Params: aParams });
            }
        }
        executeNonQueryFactory.executeNonQuery(JSON.stringify(input))
            .then(function (response) {
                $scope.status = "";
                $scope.data = response.data;
            }, function (error) {
                $scope.status = 'Unable to load customer data: ' + JSON.stringify(error.data);
            });
    };
    $scope.addParam = function (x) {
        x.Params.push({ ParameterName: "", ParameterValue: "" });
    }
    $scope.deleteParam = function (x, p) {
        x.Params.pop(p);
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