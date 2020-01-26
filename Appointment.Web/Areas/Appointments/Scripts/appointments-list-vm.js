var AppointmentsListVm = function () {
    var self = {};

    self.pageSize = 20;

    self.vm = {};

    self.init = function (appointmentsListIO, model) {
        self.appointmentsListIO = appointmentsListIO;
        self.vm = self.buildVm(model);
        self.loadAppointments();
    };

    self.buildVm = function (model) {
        return new Vue({
            el: '#appointments-list-wrapper',
            data: {
                model: model,
                appointments: [],
                totalValue: 0,
                filter: {
                    EmployeeId: '',
                    PartnerId: '',
                    TypeId: '',
                    Date: ''
                },

                pageNumber: 1,
                pageSize: self.pageSize,
                sortBy: 'Date',
                sortDirection: 'asc',
                sortColumn: {},
                loadingRecords: false,

                dateOptions: {
                    format: 'DD/MM/YYYY',
                    useCurrent: false,
                    showClose: true,
                    showClear: true,
                    locale: 'bg'
                }
            },
            methods: {
                getFormatedDate: self.getFormatedDate,
                search: self.search,
                remove: self.remove,
                clear: self.clear
            }
        });
    };

    self.getFormatedDate = function (date) {
        return moment(date, 'DD/MM/YYYY').format('DD/MM/YYYY');
    };

    self.search = function () {
        self.loadAppointments(false);
    };

    self.clear = function () {
        var vm = this;

        vm.filter = {
            EmployeeId: '',
            PartnerId: '',
            TypeId: '',
            Date: ''
        };

        self.loadAppointments(false);
    };

    self.loadAppointments = function (shouldAppend) {
        if (!shouldAppend) {
            self.vm.pageNumber = 1;
            self.vm.pageSize = self.pageSize;
            self.offsetHeight = 0;
        }

        var filters = self.getFilters();

        self.appointmentsListIO.get(filters, function (response) {
            self.vm.pageNumber += 1;
            self.vm.loadingRecords = false;
            self.vm.recordsCount = response.Count;
            self.vm.totalValue = response.TotalValue;
            self.vm.appointments = shouldAppend ? self.vm.appointments.concat(response.Records) : response.Records;
        });
    };

    self.loadMoreRecords = function () {
        if (self.recordsCount > self.vm.appointments.length) {
            self.loadAppointments(true);
        }
    };

    self.getFilters = function () {
        var vm = self.vm;

        vm.filter.PageSize = self.vm.pageSize;
        vm.filter.PageNumber = self.vm.pageNumber;
        vm.filter.SortBy = self.vm.sortBy || 'Date';
        vm.filter.SortDirection = self.vm.sortDirection || 'Asc';
        self.vm.loadingRecords = true;

        return vm.filter;
    };

    self.remove = function (appointment) {
        if (customConfirm({
            title: 'Сигурни ли сте че искате да премахнете ангажиментът на ' + appointment.EmployeeName + '?',
            okCallBack: function () {
                self.appointmentsListIO.remove(appointment.Id,
                    function () {
                        self.loadAppointments();
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

var AppointmentsListIO = function () {
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
            url: '/appointmentInfo/get',
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
            url: '/appointmentInfo/delete/' + id,
            type: 'POST',
            success: function (response) {
                self.ioCompleted(response, callBack);
            }
        });
    };

    return self;
};

$(document).ready(function () {
    var appointmentsListVm = new AppointmentsListVm();
    var appointmentsListIo = new AppointmentsListIO();

    var model = document.getElementById('model');
    appointmentsListVm.init(appointmentsListIo, JSON.parse(model.value));
    window.appointmentsListVm = appointmentsListVm;
});

window.onscroll = function () {
    window.appointmentsListVm.loadMoreRecords();
};

Vue.component('date-picker', VueBootstrapDatetimePicker);