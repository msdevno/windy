define([], function () {
    "use strict";

    var Feature = function (view, viewModel) {
        this.view = view;
        this.viewModel = viewModel;
    };


    Feature.load = function (name) {
        return new Promise(function (fulfill) {
            var prefix = "/App/";
            var viewFile = prefix + name + ".html";
            var viewModelFile = prefix + name + ".js";
            require(["text!" + viewFile, viewModelFile], function (view, viewModel) {
                var feature = new Feature(view, viewModel);
                fulfill(feature);
            })

        });
    };
    return Feature;
});



