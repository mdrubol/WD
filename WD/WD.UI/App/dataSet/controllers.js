
app.controller("dataSetController", ["$scope", "dataFactory", function ($scope, dataFactory) {
    /*root Items start*/
    rootScope.title = "WD-DataSet";
    rootScope.dataset = "active";
    rootScope.datatable = "";
    rootScope.executeNonQuery = "";
    rootScope.executeScalar = "";
    rootScope.executefilter = "";
    rootScope.executeFaq = "";
    /*root Items end*/
    $scope.authenticationKey = "UMeiS+n70CAAHQNA7TiW2g==";
        $scope.dbProviders = [
                                { text: "Sql", value: "1" },
                                { text: "Db2", value: "2" },
                                { text: "Oracle", value: "4" },
                             ];
        $scope.dbProviders2 = [
                                { text: "Db2", value: "2" },
                                { text: "Oracle", value: "4" },
                                
                              ];
        $scope.dbProvider = { text: "Sql", value: "1" };
        $scope.dbProvider2 = { text: "Db2", value: "2" };
        $scope.commandTypes = [
                            { text: "Text", value: "1" },
                            { text: "StoredProcedure", value: "4" }
        ];
        $scope.conStrings = [
                            { ConnectionName: "customized", ConnectionString: "server=172.21.42.128;database=XMMS;uid=xmms_user;password=xmms_user*123" },
                            { ConnectionName: "", ConnectionString: "" }
        ];
        $scope.conString = { ConnectionName: "", ConnectionString: "" };
        $scope.conString2 = { ConnectionName: "", ConnectionString: "" };
        $scope.data1 = [];
        $scope.data2 = [];
        $scope.theSqls = [
                            { commandText: "SELECT * FROM TempEmployee", commandType: { text: "Text", value: "1" }, Params: [] }
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
        $scope.hider2 = true;
        $scope.getDatSet = function () {
            $scope.status1 = "";
            $scope.status2 = "";
            $scope.data1 = [];
            $scope.data2 = [];
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
            dataFactory.getDataSet(JSON.stringify(input))
                .then(function (response) {
                    $scope.status1 = "";
                    $scope.data1 = response.data;
                }, function (error) {
                    $scope.data1 = [];
                    $scope.status1 = 'Unable to load customer data: ' + JSON.stringify(error.data);
                });
            input.Connect.DbType = $scope.dbProvider2.value;
            input.Connect.ConnectionString = $scope.conString2.ConnectionString;
            dataFactory.getDataSet(JSON.stringify(input))
               .then(function (response) {
                   $scope.status2 = "";
                   $scope.data2 = response.data;
               }, function (error) {
                   $scope.data2 = [];
                   $scope.status2 = 'Unable to load customer data: ' + JSON.stringify(error.data);
               });
        };
        $scope.addParam = function (x) {
            x.Params.push({ParameterName:"",ParameterValue:""});
        }
        $scope.deleteParam = function (x,p) {
            x.Params.pop(p);
        }
        $scope.getProvider = function () {
           
          
            if ($scope.dbProvider.value == "1") {
                $scope.hider = false;
                $scope.dbProviders2 = [
                                { text: "Db2", value: "2" },
                                { text: "Oracle", value: "4" }
                ];
                $scope.dbProvider2 = { text: "Db2", value: "2" };
                $scope.hider2 = true;
            }
            else if ($scope.dbProvider.value == "2") {
                $scope.dbProviders2 = [
                             { text: "Sql", value: "1" },
                             { text: "Oracle", value: "3" }
                ];
                $scope.dbProvider2 = { text: "Sql", value: "1" };
                $scope.hider = true;
                $scope.hider2 = false;
                $scope.conString = { ConnectionName: "", ConnectionString: "" };
            }
            else {
                $scope.dbProviders2 = [
                              { text: "Sql", value: "1" },
                              { text: "Db2", value: "2" }
                ];
                $scope.dbProvider2 = { text: "Sql", value: "1" };
                $scope.hider = true;
                $scope.hider2 = false;
                $scope.conString = { ConnectionName: "", ConnectionString: "" };
            }
       
        };
        $scope.getProvider2 = function () {
            if ($scope.dbProvider2.value == "1") {
                $scope.hider2 = false;
            }
            else {
                $scope.hider2 = true;
                $scope.conString2 = { ConnectionName: "", ConnectionString: "" };
            }
        };
    }]);