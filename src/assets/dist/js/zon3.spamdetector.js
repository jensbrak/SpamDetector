spamdetector = new Vue({
    el: "#spamdetector",
    data: {
        loading: true,
        model: {
            enabled: false,
            spamApiUrl: null,
            siteUrl: null,
            siteLanguage: null,
            siteEncoding: null,
            userRole: null,
            isTest: true
        }
    },
    methods: {
        load: function () {
            var self = this;

            fetch(piranha.baseUrl + "manager/api/spamdetector/list/")
                .then(function (response) { return response.json(); })
                .then(function (result) {
                    self.model.enabled = result.enabled;
                    self.model.spamApiUrl = result.spamApiUrl;
                    self.model.siteUrl = result.siteUrl;
                    self.model.siteLanguage = result.siteLanguage;
                    self.model.siteEncoding = result.siteEncoding;
                    self.model.userRole = result.userRole;
                    self.model.isTest = result.isTest;
                })
                .catch(function (error) { console.log("error:", error); });
        },
        save: function () {
            self = this;

            fetch(piranha.baseUrl + "manager/api/spamdetector/save", {
                method: "post",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    enabled: spamdetector.model.enabled,
                    spamApiUrl: spamdetector.model.spamApiUrl,
                    siteUrl: spamdetector.model.siteUrl,
                    siteLanguage: spamdetector.model.siteLanguage,
                    siteEncoding: spamdetector.model.siteEncoding,
                    userRole: spamdetector.model.userRole,
                    isTest: spamdetector.model.isTest
                })
            })
            .then(function (response) { return response.json(); })
            .then(function (result) {
                // Push status to notification hub
                notifications.push(result.status);
            })
            .catch(function (error) {
                console.log("error:", error);
            });
        },
    },
    created: function () {
        this.load();
    },
    updated: function () {
        this.loading = false;
    }
});
