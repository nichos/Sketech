var apiURL = '/api/logging/GetAuditLogs';

var app = new Vue({
    el: '#app',
    data: {
        logs: []
    },
    created: function () {
        this.fetchData();
    },
    methods: {
        fetchData: function () {
            var xhr = new XMLHttpRequest();
            var self = this;
            xhr.open('GET', apiURL);
            xhr.onload = function () {
                self.logs = JSON.parse(xhr.responseText);
            }
            xhr.send();
        }
    }
})