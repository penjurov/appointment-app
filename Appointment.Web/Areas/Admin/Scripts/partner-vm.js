var PartnersVm = function () {
    var self = {};

    self.offsetHeight = 0;
    self.pageSize = 20;

    self.vm = {};

    self.init = function (partnersIO) {
        self.partnersIO = partnersIO;
        self.vm = self.buildVm();
        self.loadPartners();
    };

    self.buildVm = function () {
        return new Vue({
            el: '#partner-wrapper',
            data: {
                partners: [],
                searchKeyword: '',
                isModalVisible: false,

                currentPartner: {},
                errors: {},

                pageNumber: 1,
                pageSize: self.pageSize,
                sortBy: 'Name',
                sortDirection: 'asc',
                sortColumn: {},
                loadingRecords: false
            },
            methods: {
                onKeywordChanged: self.onKeywordChanged,
                addPartner: self.addPartner,
                editPartner: self.editPartner,
                savePartner: self.savePartner,
                removePartner: self.removePartner
            }
        });
    };

    self.loadPartners = function (shouldAppend) {
        if (!shouldAppend) {
            self.vm.pageNumber = 1;
            self.vm.pageSize = self.pageSize;
            self.offsetHeight = 0;
        }

        var filters = self.getFilters();

        self.partnersIO.get(filters, function (response) {
            self.vm.pageNumber += 1;
            self.vm.loadingRecords = false;
            self.vm.partners = shouldAppend ? self.vm.partners.concat(response) : response;
        });
    };

    self.loadMoreRecords = function (offsetHeight) {
        if (self.offsetHeight !== offsetHeight) {
            self.offsetHeight = offsetHeight;
            self.loadPartners(true);
        }
    };

    self.onKeywordChanged = function (e) {
        var search = function () {
            clearTimeout($.data(this, 'timer'));
            $(this).data('timer', setTimeout(function () {
                self.loadPartners();
            }, 500));
        };

        self.vm.pageNumber = 1;

        //Disabled Enter, Shift, Control and Alt, Page Up + Down and Arrow keys.
        if (e.keyCode === 13) {
            e.preventDefault();
            e.stopPropagation();
            search();
        } else if (!((e.keyCode >= 16 && e.keyCode <= 18) || (e.keyCode >= 33 && e.keyCode <= 40))) {
            search();
        }
    };

    self.getFilters = function () {
        var filter = {};
        filter.Keyword = self.vm.searchKeyword;
        filter.PageSize = self.vm.pageSize;
        filter.PageNumber = self.vm.pageNumber;
        filter.SortBy = self.vm.sortBy || 'Name';
        filter.SortDirection = self.vm.sortDirection || 'Asc';
        self.vm.loadingRecords = true;

        return filter;
    }

    self.addPartner = function () {
        var vm = this;

        vm.currentPartner = {};
        vm.errors = {};
        vm.isModalVisible = true;
    };

    self.editPartner = function (partner) {
        var vm = this;

        vm.currentPartner = $.extend({}, partner);
        vm.errors = {};
        vm.isModalVisible = true;
    };

    self.savePartner = function () {
        var vm = self.vm;

        vm.errors = {};

        if (!vm.currentPartner.Name) {
            vm.errors.name = 'Името е задължително';
        }

        var existing = vm.partners.some(function (partner) {
            return partner.Name === vm.currentPartner.Name;
        });

        if (existing) {
            customAlert('Партньора вече съществува');
            return;
        }

        if (Object.keys(vm.errors).length > 0) {
            return;
        }

        self.partnersIO.upsert(vm.currentPartner, function () {
            self.loadPartners();
            vm.isModalVisible = false;
        });
    };

    self.removePartner = function (partner) {
        if (customConfirm({
            title: 'Сигурни ли сте че искате да премахнете ' + partner.Name + '?',
            okCallBack: function () {
                self.partnersIO.remove(partner.Id,
                    function () {
                        customAlert('Партньора бе премахнат');
                        self.loadPartners();
                    }
                );
            }
        }));
    };

    return {
        init: self.init,
        loadMoreRecords: self.loadMoreRecords
    };
};

var PartnersIO = function () {
    var self = {};

    self.ioCompleted = function (response, callBack) {
        if (!response.Success) {
            customAlert(response.Message);
        } else {
            if (callBack) {
                callBack(response.Data);
            }
        }
    };

    self.get = function (filters, callBack) {
        $.ajax({
            url: '/partners/get',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(filters),
            dataType: 'json',
            success: function (response) {
                self.ioCompleted(response, callBack);
            },
            error: function (response) {
                self.ioCompleted(response, callBack);
            }
        });
    };

    self.remove = function (id, callBack) {
        $.ajax({
            url: '/partners/remove/' + id,
            type: 'POST',
            success: function (response) {
                self.ioCompleted(response, callBack);
            }
        });
    };

    self.upsert = function (Partner, callBack) {
        $.ajax({
            type: 'POST',
            url: '/partners/upsert',
            data: JSON.stringify(Partner),
            dataType: 'json',
            contentType: 'application/json',
            success: function (response) {
                self.ioCompleted(response, callBack);
            },
            error: function (response) {
                self.ioCompleted(response, callBack);
            }
        });
    };

    return self;
};

$(document).ready(function () {
    var partnersVm = new PartnersVm();
    var partnersIo = new PartnersIO();
    partnersVm.init(partnersIo);
    window.partnersVm = partnersVm;
});

window.onscroll = function () {
    //Load more records if end of the page reached
    let bottomOfWindow = (document.documentElement.scrollTop + window.innerHeight) >= (document.documentElement.offsetHeight - 1);

    if (bottomOfWindow) {
        window.partnersVm.loadMoreRecords(document.documentElement.offsetHeight);
    }
};