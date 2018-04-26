app.factory('faqFactory', ['$http', function ($http) {
        $http.defaults.headers.common["Access-Control-Allow-Origin"] = "*";
        var urlBase = baseUrl +'/api/DataTable';
        var faqFactory = {};
        faqFactory.getFaq = function () {
            var input = {
                "AuthenticationKey": "UMeiS+n70CAAHQNA7TiW2g==",
                "TheSql": [{ commandText: "SELECT CommentId,Comment,ParentId FROM Comment ORDER BY CreatedOn ASC", commandType: { text: "Text", value: "1" }, Params: [] }],
                "Connect": {
                    "DbProvider": 1,
                    "ConnectionString": "",
                    "ConnectionName": ""
                }
            };
            return $http.post(urlBase, JSON.stringify(input));
        };
        faqFactory.insertFaq = function (wrapper) {
            urlBase = baseUrl + '/api/ExecuteNonQuery';
            return $http.post(urlBase, wrapper);
        };
        return faqFactory;
    }]);