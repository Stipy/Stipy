"use strict";
/*global $, jQuery, angular*/
/*jshint -W097 */

var feedbackApp = angular.module('FeedbackApp', []);

feedbackApp.controller('FeedbackListContoller', function ($scope, $http) {
    $http.get('http://localhost:8090/spity/Get?type=GetFeedbacksRequestObject')
        .success(function (data) {
            $scope.FeedbackList = data;
            for(var i=0; i<$scope.FeedbackList.length; i++)
            {
                $scope.FeedbackList[i].Number = i;
            }
        });
});


feedbackApp.directive("feedbackItem",
    function ($compile) {
        return {
            templateUrl: "FeedbackItem.html"
        };
    });