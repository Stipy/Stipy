"use strict";
/*global $, jQuery, angular*/

var app = angular.module('FeedbackApp', []);

app.controller('FeedbackListContoller', function ($scope, $http) {
    $http.get(ipAddress + '/Get?type=GetFeedbacksRequestObject')
        .success(function (data) {
            $scope.FeedbackList = getNumberedList(data);

        });
    $scope.CurrentDate = new Date();
});

app.directive("feedbackItem", function ($compile) {
    return {
        templateUrl: "FeedbackItem.html"
    };
});

app.filter("aspNetDateTime", function () {
    return function (item) {
        if (item != null) {
            return new Date(parseInt(item.substr(6)));
        }
        return "";
    }
});

function getNumberedList(data) {
    var i;
    for (i = 0; i < data.length; i++) {
        data[i].Number = i;
    }
    return data;
}