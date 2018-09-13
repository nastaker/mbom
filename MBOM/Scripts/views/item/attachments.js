; (function () {
    'use strict';

    var url = '/Item/UploadAttachment',
        delUrl = '/Item/DeleteAttachment';


    angular.module('demo', [
        'blueimp.fileupload'
    ])
    .config([
        '$httpProvider', 'fileUploadProvider', '$locationProvider',
        function ($httpProvider, fileUploadProvider, $locationProvider ) {
            delete $httpProvider.defaults.headers.common['X-Requested-With'];
            $locationProvider.html5Mode({
                enabled: true,
                requireBase: false
            });
            angular.extend(fileUploadProvider.defaults, {
                disableImageResize: /Android(?!.*Chrome)|Opera/
                    .test(window.navigator.userAgent),
                maxFileSize: 999000,
                acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i
            });
        }
    ])
    .controller('DemoFileUploadController', [
        '$scope', '$http', '$location',
        function ($scope, $http, $location) {
            $scope.options = {
                url: url
            };
            $scope.readonly = !!$location.search().readonly;
            $scope.loadingFiles = true;
            $http.post(url, { prod_itemcode: $location.search().prod_itemcode, filetype: $location.search().filetype })
            .then(
                function (response) {
                    $scope.loadingFiles = false;
                    console.log(response.data);
                    $scope.queue = response.data.data || [];
                },
                function () {
                    $scope.loadingFiles = false;
                }
            );
        }
    ])
    .controller('FileDestroyController', [
        '$scope', '$http', '$location',
        function ($scope, $http, $location) {
            var file = $scope.file,
                state;
            if (file.url) {
                file.$state = function () {
                    return state;
                };
                file.$destroy = function () {
                    state = 'pending';
                    return $http.post(
                        delUrl,
                        { id: file.id }
                    ).then(
                        function () {
                            state = 'resolved';
                            $scope.clear(file);
                        },
                        function () {
                            state = 'rejected';
                        }
                    );
                };
            } else if (!file.$cancel && !file._index) {
                file.$cancel = function () {
                    $scope.clear(file);
                };
            }
        }
    ]);

}());
