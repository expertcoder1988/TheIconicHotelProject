$(document).ready(function () {
    ko.applyBindings(new Features());
});

function Features() {

    var self = this;

    self.initialize = function () {

    }

    self.showIndex = function (vm) {
        window.location.href = '/Property/Index';
    };

    self.initialize();
}
