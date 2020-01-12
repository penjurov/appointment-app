var AddAppointmentVm = function () {
    var self = {};
    self.vm = {};

    self.init = function (addAppointmentIo, model) {
        self.addAppointmentIo = addAppointmentIo;
        self.vm = self.buildVm(model);
    };

    self.buildVm = function (model) {
        return new Vue({
            el: '#add-appointment-wrapper',
            data: {
                model: model || {},
                appointment: {},
                errors: {},
                dateOptions: {
                    format: 'MM/DD/YYYY',
                    useCurrent: false,
                    showClose: true,
                    showClear: true,
                    locale: 'bg'
                }
            },
            methods: {
                save: self.save,
                clear: self.clear
            }
        });
    };

    self.save = function () {
        var vm = self.vm;

        vm.errors = {};

        if (!vm.appointment.EmployeeId) {
            vm.errors.employee = 'Изберете служител';
        }

        if (!vm.appointment.PartnerId) {
            vm.errors.partner = 'Изберете партньор';
        }

        if (!vm.appointment.TypeId) {
            vm.errors.type = 'Изберете тип';
        }

        if (!vm.appointment.Date) {
            vm.errors.date = 'Изберете дата';
        }

        if (Object.keys(vm.errors).length > 0) {
            return;
        }

        self.addAppointmentIo.save(vm.appointment, function () {
            self.clear();
        });
    };

    self.clear = function () {
        var vm = self.vm;

        vm.appointment = {};
        vm.errors = {};
    };

    return {
        init: self.init
    };
};

var AddAppointmentIo = function () {
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

    self.save = function (appointment, callBack) {
        $.ajax({
            type: 'POST',
            url: '/appointmentinfo/save',
            data: JSON.stringify(appointment),
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
    var addAppointmentVm = new AddAppointmentVm();
    var addAppointmentIo = new AddAppointmentIo();

    var model = document.getElementById('model');
    addAppointmentVm.init(addAppointmentIo, JSON.parse(model.value));
});

Vue.component('date-picker', VueBootstrapDatetimePicker);