(function () {
    'use strict';

    var app = angular.module('app');

    app.controller('FriendCtrl', FriendCtrl);

    FriendCtrl.$inject = ['$scope', 'DataService'];

    function FriendCtrl($scope, DataService)
    {
        $scope.inputText = "";
        $scope.messages = [{
            'Body': 'What can I help you with?',
            'Businesses': [],
            'fromUser': false
        }];

        $scope.keydown = keydown;

        function keydown(event) {
            if (event.keyCode == 13 && $scope.inputText != "") {
                send();
            }
        }

        function send() {
            var query = {
                'Body': $scope.inputText,
            };

            createMessage({ 'Body': $scope.inputText, 'Businesses': [] }, true);
            $scope.inputText = "";

            DataService.retrieveMessage(query)
            .then(function (data) {
                createMessage(data.data, false);
            })
            .catch(function (error) {
                console.log('Retrieve Failed', error);
            });
        }

        function createMessage(message, fromUser) {
            var newMessage = {
                'Body': message.Body,
                'Businesses': message.Businesses,
                'fromUser': fromUser
            };

            $scope.messages.push(newMessage);

            updateScroll();
        }

        function updateScroll() {
            $("#message-container").animate({ scrollTop: $('#message-container')[0].scrollHeight }, 300);
        }
    }
})();