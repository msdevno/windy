define([], function () {

    var baseUrl = "https://invoiceapproval.compello.com/RestService.svc";

    var server = function () {

        this.post = function (action, parameters) {
            return new Promise(function (fulfill, failed) {
                var serviceUrl = baseUrl + "/" + action;

                $.ajax(serviceUrl, {
                    data: JSON.stringify(parameters),
                    global: false,
                    async: true,
                    type: 'POST',
                    contentType: 'application/json'
                }).fail(function (result) {
                    failed(result);
                }).done(function (result) {
                    fulfill(result)
                });
            });
        };
    };

    return new server();
});