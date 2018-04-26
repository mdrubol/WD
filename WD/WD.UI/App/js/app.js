var app = angular.module("myApp", ['ui.router']);
var rootScope;
var baseUrl = "http://172.21.12.166:100";
app.run(["$rootScope", function ($rootScope) {
    $rootScope.title = "WD-Home";
    rootScope = $rootScope;
}]);
app.config(["$stateProvider", "$urlRouterProvider", "$locationProvider", function ($stateProvider,$urlRouterProvider, $locationProvider) {
  
    $stateProvider
    .state("executeFaq", {
        templateUrl: "App/Faq/Index.html",
        controller: "faqController"
    })
     .state("/", {
        templateUrl: "/App/Home.html",
        controller: "homeController"
    }).state("DataSet", {
        templateUrl: "/App/dataSet/index.html",
        controller: "dataSetController"
    }).state("DataTable", {
        templateUrl: "/App/DataTable/index.html",
        controller: "dataTableController"
    })
    .state("executeNonQuery", {
        templateUrl: "/App/executeNonQuery/index.html",
        controller: "executeNonQueryController"
    }).state("executeScalar", {
        templateUrl: "/App/executeScalar/index.html",
        controller: "executeScalarController"
    }).state("executeEncrypt", {
        templateUrl: "/App/executeEncrypt/index.html",
        controller: "executeEncryptController"
    }).state("executeFilter", {
        templateUrl: "ConvertStatements.aspx?OP=OL",
    });
    $urlRouterProvider.otherwise("/");
    $locationProvider.html5Mode({
        enabled: true
    }).hashPrefix("#");
  
}]);

