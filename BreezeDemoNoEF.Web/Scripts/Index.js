(function() {
    if (config && dataServiceFactory) {
        var viewModel = new viewModels.OrdersViewModel({ mode: 'read-only' });
        var element = $('#mainDiv')[0];

        viewModel.init(function() {
             ko.applyBindings(viewModel, element);
        });
        
    }
})();