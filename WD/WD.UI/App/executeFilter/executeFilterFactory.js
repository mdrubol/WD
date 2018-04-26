app.factory('executeFilterFactory', ['$http', function ($http) {
    var urlBase = "http://sqlines.com/online/sqlines_run.php";
    var executeFilter = {};
    executeFilter.encrypt = function (param) {
        return $http.post(urlBase, param);
    };
    return executeFilter;
}]);