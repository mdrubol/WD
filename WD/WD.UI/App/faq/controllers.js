app.controller("faqController", ["$scope", "faqFactory", function ($scope, faqFactory) {
    /*root Items start*/
    rootScope.title = "WD-DataSet";
    rootScope.dataset = "";
    rootScope.datatable = "";
    rootScope.executeNonQuery = "";
    rootScope.executeScalar = "";
    rootScope.executefilter = "";
    rootScope.executeFaq = "active";
    /*root Items end*/
    $scope.child = [];
    $scope.faq = [];
    $scope.initialize = function () {
        faqFactory.getFaq()
                .then(function (response) {
                    $scope.faq = response.data;
                }, function (error) {
                    $scope.alert = "alert-danger";
                    $scope.status = 'Unable to load customer data: ' + JSON.stringify(error.data);
                });
       
    };
    $scope.email = "";
    $scope.question = "";
    $scope.submit = function () {
      var  query = {
            "AuthenticationKey": "UMeiS+n70CAAHQNA7TiW2g==",
            "TheSql": [{ commandText: "INSERT INTO Comment(Comment,CreatedBy) VALUES('" + $scope.question + "','" + $scope.email + "')", commandType: { text: "Text", value: "1" }, Params: [] }],
            "Connect": {
                "DbProvider": 1,
                "ConnectionString": "",
                "ConnectionName": ""
            }
      };
        faqFactory.insertFaq(JSON.stringify(query))
          .then(function (response) {
              $scope.alert = "alert-success";
              $scope.status = "question submitted successfully.";
             
          }, function (error) {
              $scope.alert = "alert-danger";
              $scope.status = 'error while submitting: ' + JSON.stringify(error.data);
          });
        $scope.faq.push({
            commentId: -1,
            comment: $scope.question
        });
    };
    $scope.initialize();
}]);