
app.factory('executeNonQueryFactory', ['$http', function ($http) {
    var urlBase = baseUrl + '/api/ExecuteNonQuery';
    var executeNonQueryFactory = {};
    executeNonQueryFactory.executeNonQuery = function (wrapper) {
        return $http.post(urlBase, wrapper);
    };
    return executeNonQueryFactory;
}]);