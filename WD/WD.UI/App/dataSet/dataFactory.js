
app.factory('dataFactory', ['$http', function ($http) {
        $http.defaults.headers.common["Access-Control-Allow-Origin"] = "*";
        var urlBase = baseUrl +'/api/DataSet';
        var dataFactory = {};
        dataFactory.getDataSet = function (wrapper) {
            return $http.post(urlBase, wrapper);
        };
        return dataFactory;
    }]);