(function() {
    if (config && entityManagerFactory) {
        var dataService = entityManagerFactory(config).newManager;
        var viewModel = new viewModels.OrdersViewModel({ results: [], mode: 'read-only' });
        var element = $('#mainDiv')[0];

        var query = new breeze.EntityQuery()
            .from("Orders")
            .using(dataService);

        var getFilteredDataFromLocal = function() {
            var query2 = query.where("Customer", "eq", "Customer1")
                .using(breeze.FetchStrategy.FromLocalCache);

            dataService.executeQuery(query2).then(bindData);
        };

        var getFilteredDataFromServer = function () {
            var query2 = query.where("Customer", "eq", "Customer1");
            query2.execute().then(bindData);
        };

        var getOrderDetails = function (orderId) {
            var query2 = query.where("Id", "eq", orderId)
                .expand("OrderDetails");

            dataService.executeQuery(query2).then(function(data) {
                viewModel.orderDetails(data.results[0].OrderDetails());
            });
        };

        var bindData = function (data) {
            viewModel.orders(data.results);
        };

        query.execute().then(function (data) {
            bindData(data);
            ko.applyBindings(viewModel, element);
            $('.orderId').on('click', function () {
                getOrderDetails(ko.dataFor(this).Id());
            });
        }).fail(function (e) {
            alert(e);
        });

        //Event handlers
        $('#btnEdit').on('click', function() {
            viewModel.mode('edit');
        });

        $('#btnSave').on('click', function () {
            dataService.saveChanges(null, null);
            viewModel.mode('read-only');
        });

        $('#btnCancel').on('click', function () {
            viewModel.mode('read-only');
        });

        $('#btnFilter').on('click', function () {
           getFilteredDataFromLocal();
           //getFilteredDataFromServer();
        });

        $('#btnClearFilter').on('click', function () {
            var queryTemp = query.using(breeze.FetchStrategy.FromLocalCache);

            dataService.executeQuery(queryTemp).then(bindData);
        });
    }
})();