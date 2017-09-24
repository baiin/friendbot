(function () {
    'use strict';

    var app = angular.module('app');

    app.factory('DataService', DataService);

    DataService.$inject = ['$http', '$q'];

    function DataService($http, $q) {
        var service = {};

        service.retrieveMessage = retrieveMessage;

        return service;
        
        function httpPost(url, data) {
            var defer = $q.defer();

            $http.post(url, data)
            .then(function (data) {
                defer.resolve(data);
            })
            .catch(function (error) {
                defer.reject(error);
            });

            return defer.promise;
        }

        function retrieveMessage(query) {
            //var url = "http://localhost:14485/api/friend/";
            var url = "http://friendbotapi-dev.us-west-1.elasticbeanstalk.com/api/friend/";

            return httpPost(url, query);
        }
    }
})();