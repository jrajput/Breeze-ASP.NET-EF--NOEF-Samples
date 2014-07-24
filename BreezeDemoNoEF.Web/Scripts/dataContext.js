var datacontext = (function() {
    var dataService = dataServiceFactory(config);
    var manager = dataService.newManager();
    var filterQueryOp = breeze.FilterQueryOp;

    var query = new breeze.EntityQuery()
       .from("Orders")
       .orderBy("Customer")
       .toType("Order")
       .using(manager);

    var getAllOrders = function (ordersObservable, successCallBack) {
        query.execute().then(function (data) {
            successCallBack(data.results);
        }).fail(function (e) {
            alert(e);
        });
    };

    var getAllLocalOrders = function() {
        return manager.getEntities("Order");
    }

    var getOrderDetails = function (order) {
        //var query2 = new breeze.EntityQuery().from("OrderDetails").where("OrderId", "eq", orderId)
        //    .toType("OrderDetail");

        //var query2 = new breeze.EntityQuery().from("OrderDetails").where("OrderId", "eq", orderId)
        //    .toType("OrderDetail").using(breeze.FetchStrategy.FromLocalCache);

        return order.getProperty("OrderDetails");

        //manager.executeQuery(query2).then(function(data) {
        //    viewModel.orderDetails(data.results);
        //});
    };

    var getFilteredDataFromLocal = function (ordersObservable, filterValue) {
        if (filterValue) {
            var query2 = query.where("Customer", filterQueryOp.StartsWith, filterValue)
                .using(breeze.FetchStrategy.FromLocalCache);

            manager.executeQuery(query2).then(function(data) {
                ordersObservable(data.results);
            });
        }
    };

    var getFilteredDataFromServer = function (ordersObservable, filterValue) {
        if (filterValue) {
            var query2 = query.where("Customer", filterQueryOp.StartsWith, filterValue);
            query2.execute().then(function(data) {
                ordersObservable(data.results);
            });
        }
    };

    var getSortedOrders = function (orderColumn, sortCallBack, direction) {
        var sortQuery;
        if (direction === 'DESC') {
            sortQuery = new breeze.EntityQuery().from("Orders").orderByDesc(orderColumn).using(breeze.FetchStrategy.FromLocalCache);
        }
        else{
            sortQuery = new breeze.EntityQuery().from("Orders").orderBy(orderColumn).using(breeze.FetchStrategy.FromLocalCache);
        }
        manager.executeQuery(sortQuery).then(function (data) {
            sortCallBack(data.results);
        }).fail(function (e) {
            alert(e);
        });
    };

    var saveChanges = function (successCallBack, failCallBack) {
        if (manager.hasChanges()) {
            manager.saveChanges().then(function (data) {
                manager.acceptChanges();
                successCallBack();
            }).catch(saveFailed);

            function saveFailed(error) {
                var errorMsgs = [];
                if (!error.entityErrors) {
                    errorMsgs.push(error.message);
                }
                else{
                    error.entityErrors.forEach(function (entErr) {
                        errorMsgs.push(entErr.errorMessage);
                    });
                }
                failCallBack(errorMsgs);
            }
        }
    };

    var addOrder = function (newCustObservable, newOrderDateObservable, failCallBack) {
        if (newCustObservable && newOrderDateObservable) {

            var orderType = manager.metadataStore.getEntityType('Order'); 
            var newOrder = orderType.createEntity('Order');
            newOrder.Customer(newCustObservable);
            newOrder.OrderDate(newOrderDateObservable);
            if (newOrder.entityAspect.validateEntity()) {
                manager.addEntity(newOrder);
            } else {
                var errorMsgs = [];
                newOrder.entityAspect.getValidationErrors().forEach(function (err) {
                    errorMsgs.push(err.errorMessage);
                });
                failCallBack(errorMsgs);
            }
        }
    };

    var deleteOrder = function (orderToDelete, successCallBack) {
        orderToDelete.entityAspect.setDeleted();
        saveChanges(successCallBack);
    };

    return {
        getAllOrders: getAllOrders,
        getOrderDetails: getOrderDetails,
        getFilteredDataFromLocal: getFilteredDataFromLocal,
        saveChanges: saveChanges,
        addOrder: addOrder,
        deleteOrder: deleteOrder,
        getAllLocalOrders: getAllLocalOrders,
        getSortedOrders: getSortedOrders
    };

}())