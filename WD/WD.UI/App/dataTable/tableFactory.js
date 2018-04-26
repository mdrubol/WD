
app.factory('tableFactory', ['$http', function ($http) {
    var urlBase = baseUrl + '/api/DataTable';
    var tableFactory = {};
    tableFactory.getDataTable = function (wrapper) {
        return $http.post(urlBase, wrapper);
    };
    return tableFactory;
}]);