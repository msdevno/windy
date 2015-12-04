define(["ko", "infrastructure/server"], function (ko, server) {
    "use strict";

    var authentication = function () {
        var self = this;

        var currentUser = "";

        var hasStorage = typeof (Storage) !== "undefined";
        var loggedIn = ko.observable(false);

        var loggingIn = ko.observable(false);

        function storeLoginInfo(customer, username, password) {
            if (!hasStorage) return;

            localStorage.customer = customer;
            localStorage.username = username;
            localStorage.password = password;
        }

        this.hasCredentialsStored = function () {
            return localStorage.customer != null && typeof (localStorage.customer) != "undefined" &&
                localStorage.username != null && typeof (localStorage.username) != "undefined" &&
                localStorage.password != null && typeof (localStorage.password) != "undefined";
        };

        this.getCurrentUser = function () {
            return currentUser;
        };

        this.login = function (customer, username, password) {
            return new Promise(function (fulfill, failed) {
                var credentials = {
                    cci: customer,
                    username: username,
                    password: password
                };

                server.post("Login", credentials).then(function(result) {
                    var authResult = result.LoginResult;

                    if (authResult !== null && authResult.ErrorMessage !== "10484") {
                        currentUser = result.LoginResult;

                        storeLoginInfo(customer, username, password);

                        loggedIn(true);
                        fulfill();
                    } else {
                        currentUser = null;
                        loggedIn(false);
                        failed();
                    }
                }).catch(failed);
            });
        };

        this.logout = function () {
            localStorage.customer = null;
            localStorage.username = null;
            localStorage.password = null;
            loggedIn(false);
        };

        this.isLoggedIn = function () {
            return new Promise(function (fulfill, failed) {
                if (loggedIn() == true) fulfill(true);
                else if (loggingIn() == true) {
                    loggingIn.subscribe(function () {
                        fulfill(loggedIn());
                    });
                } else {
                    failed();
                }
            });
        };


        if (this.hasCredentialsStored()) {
            loggingIn(true);
            self.login(
                localStorage.customer,
                localStorage.username,
                localStorage.password
            ).then(function () {
                loggingIn(false);
            }).catch(function () {
                loggingIn(false);
            });
        }
    };


    return new authentication();
});