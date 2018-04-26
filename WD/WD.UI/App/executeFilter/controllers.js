
app.controller("executeFilterController", ["$scope", "executeFilterFactory", function ($scope, executeFilterFactory) {
    /*root Items start*/
    rootScope.title = "WD-filter";
    rootScope.dataset = "";
    rootScope.datatable = "";
    rootScope.executeNonQuery = "";
    rootScope.executeScalar = "";
    rootScope.executeEncrypt = "";
    rootScope.executeFilter = "active";
    rootScope.executeFaq = "";
    $scope.sources = ["Oracle", "IBM DB2", "Microsoft SQL Server"];
    $scope.source_type = "";
    $scope.source = "";
    $scope.target_type = "";

    $scope.executeFilter = function () {
        var xmlhttp = getXMLHttpRequest();
  
        if (xmlhttp) {
            var params = "source=" + encodeURIComponent($scope.source) +
                             "&source_type=" + $scope.source_type +
                             "&target_type=" + $scope.target_type;
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                    alert(xmlhttp.responseText.split("__SQLINES_MULTI_PART__"));
                   // editAreaLoader.setValue("target", response[0]);
                   // document.getElementById("full_report_link_id").innerHTML = response[1];
                }

            }
            xmlhttp.open("POST", "http://sqlines.com/sqlines_run.php", false);
            xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
            //xmlhttp.setRequestHeader("Content-length", params.length);
            //xmlhttp.setRequestHeader("Connection", "close");
            xmlhttp.send(params);

        }
    };
    function getXMLHttpRequest() {
        var xmlhttp;
        try {
            // Firefox, Chrome, IE 7+, Opera 8.0+, Safari
            xmlhttp = new XMLHttpRequest();
        } catch (e) {
            // IE 6 and earlier
            try { xmlhttp = new ActiveXObject("Msxml2.XMLHTTP"); }
            catch (e) {
                try { xmlhttp = new ActiveXObject("Microsoft.XMLHTTP"); }
                catch (e) { alert("Your browser does not support XMLHttpRequest!"); return false; }
            }
        }
        return xmlhttp;
    }
}]);