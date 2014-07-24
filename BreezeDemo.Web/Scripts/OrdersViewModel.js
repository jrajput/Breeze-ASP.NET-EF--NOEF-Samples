(function () {
    'use strict';

    var OrdersViewModel = function (initData) {
        var self = this;

        if (!(self instanceof OrdersViewModel)) {
            throw new Error('OrdersViewModel must be called with a new keyword');
        }        
        self.orders = ko.observableArray([]);
        self.mode = ko.observable(initData.mode);
        self.orderDetails = ko.observableArray([]);

        self.orders(initData.results);
    };

    OrdersViewModel.prototype = $.extend(OrdersViewModel.prototype, (function () {

        return {
            
        };
    }()));

    if (typeof window.viewModels === 'undefined') {
        window.viewModels = {
            OrdersViewModel: OrdersViewModel
        };
    } else {
        $.extend(window.viewModels, {
            OrdersViewModel: OrdersViewModel
        });
    }
}());