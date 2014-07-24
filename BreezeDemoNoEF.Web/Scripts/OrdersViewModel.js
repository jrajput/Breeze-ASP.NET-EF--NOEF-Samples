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
        self.filterValue = ko.observable();
        self.sortOrder = 'ASC';
        self.newCustomer = ko.observable();
        self.newOrderDate = ko.observable();
        self.isError = ko.observable(false);
        self.errorMessages = ko.observable(['']);
    };

    OrdersViewModel.prototype = $.extend(OrdersViewModel.prototype, (function () {
        var self;
        var onEdit = function () {
            self.mode('edit');
        };
        var onCancel = function () {
            self.mode('read-only');
        };
        var onAdd = function () {
            self.mode('Add');
        };
        var onFilter = function() {
            datacontext.getFilteredDataFromLocal(self.orders, self.filterValue());
        };
        var onClearFilter = function() {
            self.orders(datacontext.getAllLocalOrders());
            self.filterValue(undefined);
        };
        var onSave = function () {
            if (self.mode() === 'Add') {
                datacontext.addOrder(self.newCustomer(), self.newOrderDate(), function(error) {
                    self.errorMessages(['']);
                    self.isError(true);
                    self.errorMessages(error);
                });
            }
            datacontext.saveChanges(function() {
                self.orders(datacontext.getAllLocalOrders());
                self.mode('read-only');
                self.errorMessages(['']);
                self.isError(false);
            }, function (error) {
                self.errorMessages(['']);
                self.isError(true);
                self.errorMessages(error);
            });
        };
        var onSort = function () {
            if (self.sortOrder === 'DESC')
                self.sortOrder = 'ASC';
            else {
                self.sortOrder = 'DESC';
            }
            if (self.mode() === 'read-only') {
                datacontext.getSortedOrders('Customer', function(res) {
                    self.orders(res);
                }, self.sortOrder);
            }
        };
        var getOrderDetails = function(order) {
            var odets = datacontext.getOrderDetails(order);
            self.orderDetails(odets);
        };
        var deleteOrder = function (order) {
            datacontext.deleteOrder(order, function () {
                self.orders(datacontext.getAllLocalOrders());
            });
        };
        var init = function (successCallBack) {
            self = this;
            datacontext.getAllOrders(self.orders, function(results) {
                self.orders(results);
                successCallBack();
            });
        };
        return {
            init: init,
            getOrderDetails: getOrderDetails,
            deleteOrder: deleteOrder,
            onSave: onSave,
            onEdit: onEdit,
            onCancel: onCancel,
            onAdd: onAdd,
            onFilter: onFilter,
            onSort: onSort,
            onClearFilter: onClearFilter
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