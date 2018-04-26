app.factory('executeEncryptFactory', ['$http', function ($http) {
    var urlBase = baseUrl + '/api/Encrypt';
    var executeEncrypt = {};
    executeEncrypt.encrypt = function (conString) {
        return $http.get(urlBase + '?conString=' + conString);
    };
    return executeEncrypt;
}]);