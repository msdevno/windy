define(["ko", "infrastructure/Feature", "infrastructure/attributeParser", "infrastructure/authentication"], function (ko, Feature, attributeParser, authentication) {
    "use strict";

    var navigation = function () {
        var self = this;
        var navigationFrameAttributeName = "data-navigation-frame";
        var navigationFrame = document.querySelector("["+navigationFrameAttributeName+"]");
        var navigationFrameAttribute = navigationFrame.attributes.getNamedItem(navigationFrameAttributeName);
        var navigationFrameSettings = attributeParser.parse(navigationFrameAttribute.value);
        var loginPath = "";

        function setDocumentHash(target) {
            if (target.indexOf("/") != 0) target = "/" + target;
            document.location.hash = target;
        }

        function handleNavigateTo(target) {
            if (navigationFrame) {
                setDocumentHash(target);

                if (target.indexOf("/") == 0) target = target.substr(1);

                Feature.load(target).then(function (feature) {
                    navigationFrame.innerHTML = feature.view;
                    ko.cleanNode(navigationFrame);
                    ko.applyBindings(new feature.viewModel(), navigationFrame);
                });
            }
        }

        this.setLoginPath = function (path) {
            loginPath = path;
        };

        this.navigateTo = function (target) {
            authentication.isLoggedIn().then(function (result) {
                if (result == true) {
                    handleNavigateTo(target);
                } else {
                    handleNavigateTo(loginPath);
                }
            }).catch(function () {
                handleNavigateTo(loginPath);
            });
        };

        this.navigateToLogin = function () {
            self.navigateTo(loginPath);
        };

        function handleHash() {
            var target = document.location.hash.split("#").join("");
            if (target.length > 0) self.navigateTo(target);
            else self.navigateTo(navigationFrameSettings.home);
        }

        handleHash();

        window.addEventListener("hashchange", handleHash);
    };

    return new navigation();
});