
app.factory('executeScalarFactory', ['$http', function ($http) {
    var urlBase = baseUrl + '/api/ExecuteScalar';
    var executeScalarFactory = {};
    executeScalarFactory.executeScalar = function (wrapper) {
        return $http.post(urlBase, wrapper);
    };
    return executeScalarFactory;
}]);