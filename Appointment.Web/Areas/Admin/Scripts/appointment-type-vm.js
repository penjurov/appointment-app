var AppointmentTypeVm = function () {
    var self = {};

    self.offsetHeight = 0;
    self.pageSize = 20;

    self.vm = {};

    self.init = function (appointmentTypeIO) {
        self.appointmentTypeIO = appointmentTypeIO;
        self.vm = self.buildVm();
        self.loadAppointmentTypes();
    };

    self.buildVm = function () {
        return new Vue({
            el: '#appointment-type-wrapper',
            data: {
                appointmentTypes: [],
                searchKeyword: '',
                isModalVisible: false,

                currentAppointmentType: {},
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
                addAppointmentType: self.addAppointmentType,
                editAppointmentType: self.editAppointmentType,
                saveAppointmentType: self.saveAppointmentType,
                removeAppointmentType: self.removeAppointmentType
            }
        });
    };

    self.loadAppointmentTypes = function (shouldAppend) {
        if (!shouldAppend) {
            self.vm.pageNumber = 1;
            self.vm.pageSize = self.pageSize;
            self.offsetHeight = 0;
        }

        var filters = self.getFilters();

        self.appointmentTypeIO.get(filters, function (response) {
            self.vm.pageNumber += 1;
            self.vm.loadingRecords = false;
            self.vm.appointmentTypes = shouldAppend ? self.vm.appointmentTypes.concat(response) : response;
        });
    };

    self.loadMoreRecords = function (offsetHeight) {
        if (self.offsetHeight !== offsetHeight) {
            self.offsetHeight = offsetHeight;
            self.loadAppointmentTypes(true);
        }
    };

    self.onKeywordChanged = function (e) {
        var search = function () {
            clearTimeout($.data(this, 'timer'));
            $(this).data('timer', setTimeout(function () {
                self.loadAppointmentTypes();
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

    self.addAppointmentType = function () {
        var vm = this;

        vm.currentAppointmentType = {};
        vm.errors = {};
        vm.isModalVisible = true;
    };

    self.editAppointmentType = function (appointmentType) {
        var vm = this;

        vm.currentAppointmentType = $.extend({}, appointmentType);
        vm.errors = {};
        vm.isModalVisible = true;
    };

    self.saveAppointmentType = function () {
        var vm = self.vm;

        vm.errors = {};

        if (!vm.currentAppointmentType.Name) {
            vm.errors.name = 'Името е задължително';
        }

        if (!vm.currentAppointmentType.Value) {
            vm.errors.name = 'Стойността е задължителна';
        }

        var existing = vm.appointmentTypes.some(function (appointmentType) {
            return appointmentType.Name === vm.currentAppointmentType.Name && appointmentType.Id !== vm.currentAppointmentType.Id;
        });

        if (existing) {
            customAlert('Този вид ангажимент вече съществува');
            return;
        }

        if (Object.keys(vm.errors).length > 0) {
            return;
        }

        self.appointmentTypeIO.upsert(vm.currentAppointmentType, function () {
            self.loadAppointmentTypes();
            vm.isModalVisible = false;
        });
    };

    self.removeAppointmentType = function (appointmentType) {
        if (customConfirm({
            title: 'Сигурни ли сте че искате да премахнете ' + appointmentType.Name + '?',
            okCallBack: function () {
                self.appointmentTypeIO.remove(appointmentType.Id,
                    function () {
                        customAlert('Видът ангажимент бе премахнат');
                        self.loadAppointmentTypes();
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

var AppointmentTypeIO = function () {
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
            url: '/appointmentType/get',
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
            url: '/appointmentType/remove/' + id,
            type: 'POST',
            success: function (response) {
                self.ioCompleted(response, callBack);
            }
        });
    };

    self.upsert = function (appointmentType, callBack) {
        $.ajax({
            type: 'POST',
            url: '/appointmentType/upsert',
            data: JSON.stringify(appointmentType),
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
    var appointmentTypeVm = new AppointmentTypeVm();
    var appointmentTypeIo = new AppointmentTypeIO();
    appointmentTypeVm.init(appointmentTypeIo);
    window.appointmentTypeVm = appointmentTypeVm;
});

window.onscroll = function () {
    //Load more records if end of the page reached
    let bottomOfWindow = (document.documentElement.scrollTop + window.innerHeight) >= (document.documentElement.offsetHeight - 1);

    if (bottomOfWindow) {
        window.appointmentTypeVm.loadMoreRecords(document.documentElement.offsetHeight);
    }
};