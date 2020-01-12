var EmployeesVm = function () {
    var self = {};

    self.offsetHeight = 0;
    self.pageSize = 20;

    self.vm = {};

    self.init = function (employeesIO) {
        self.employeesIO = employeesIO;
        self.vm = self.buildVm();
        self.loadEmployees();
    };

    self.buildVm = function () {
        return new Vue({
            el: '#employees-wrapper',
            data: {
                employees: [],
                searchKeyword: '',
                isModalVisible: false,

                currentEmployee: {},
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
                addEmployee: self.addEmployee,
                editEmployee: self.editEmployee,
                saveEmployee: self.saveEmployee,
                removeEmployee: self.removeEmployee
            }
        });
    };

    self.loadEmployees = function (shouldAppend) {
        if (!shouldAppend) {
            self.vm.pageNumber = 1;
            self.vm.pageSize = self.pageSize;
            self.offsetHeight = 0;
        }

        var filters = self.getFilters();

        self.employeesIO.get(filters, function (response) {
            self.vm.pageNumber += 1;
            self.vm.loadingRecords = false;
            self.vm.employees = shouldAppend ? self.vm.employees.concat(response) : response;
        });
    };

    self.loadMoreRecords = function (offsetHeight) {
        if (self.offsetHeight !== offsetHeight) {
            self.offsetHeight = offsetHeight;
            self.loadEmployees(true);
        }
    };

    self.onKeywordChanged = function (e) {
        var search = function () {
            clearTimeout($.data(this, 'timer'));
            $(this).data('timer', setTimeout(function () {
                self.loadEmployees();
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

    self.addEmployee = function () {
        var vm = this;

        vm.currentEmployee = {};
        vm.errors = {};
        vm.isModalVisible = true;
    };

    self.editEmployee = function (employee) {
        var vm = this;

        vm.currentEmployee = $.extend({}, employee);
        vm.errors = {};
        vm.isModalVisible = true;
    };

    self.saveEmployee = function () {
        var vm = self.vm;

        vm.errors = {};

        if (!vm.currentEmployee.Name) {
            vm.errors.name = 'Името е задължително';
        }

        var existing = vm.employees.some(function (employee) {
            return employee.Name === vm.currentEmployee.Name;
        });

        if (existing) {
            customAlert('Служителя вече съществува');
            return;
        }

        if (Object.keys(vm.errors).length > 0) {
            return;
        }

        self.employeesIO.upsert(vm.currentEmployee, function () {
            self.loadEmployees();
            vm.isModalVisible = false;
        });
    };

    self.removeEmployee = function (employee) {
        if (customConfirm({
            title: 'Сигурни ли сте че искате да премахнете ' + employee.Name + '?',
            okCallBack: function () {
                self.employeesIO.remove(employee.Id,
                    function () {
                        customAlert('Служителя бе премахнат');
                        self.loadEmployees();
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

var EmployeesIO = function () {
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
            url: '/employees/get',
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
            url: '/employees/remove/' + id,
            type: 'POST',
            success: function (response) {
                self.ioCompleted(response, callBack);
            }
        });
    };

    self.upsert = function (employee, callBack) {
        $.ajax({
            type: 'POST',
            url: '/employees/upsert',
            data: JSON.stringify(employee),
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
    var employeesVm = new EmployeesVm();
    var employeesIo = new EmployeesIO();
    employeesVm.init(employeesIo);
    window.employeesVm = employeesVm;
});

window.onscroll = function () {
    //Load more records if end of the page reached
    let bottomOfWindow = (document.documentElement.scrollTop + window.innerHeight) >= (document.documentElement.offsetHeight - 1);

    if (bottomOfWindow) {
        window.employeesVm.loadMoreRecords(document.documentElement.offsetHeight);
    }
};