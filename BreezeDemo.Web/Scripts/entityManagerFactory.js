var entityManagerFactory = (function () {
    'use strict';

    return function emFactory(config) {
        // Convert server-side PascalCase to client-side camelCase property names
        //breeze.NamingConvention.camelCase.setAsDefault();

        // Do not validate when we attach a newly created entity to an EntityManager.
        // We could also set this per entityManager
        new breeze.ValidationOptions({ validateOnAttach: false }).setAsDefault();

        var metadataStore = new breeze.MetadataStore();

        var provider = {
            metadataStore: metadataStore,
            newManager: newManager()
        };

        return provider;

        function newManager() {
            var mgr = new breeze.EntityManager({
                serviceName: config.remoteServiceName,
                metadataStore: metadataStore
            });
            return mgr;
        }
    }
})();