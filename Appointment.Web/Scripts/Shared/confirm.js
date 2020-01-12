var customConfirm = (function () {
    var self = {};

    self.vm = {};

    self.init = function () {
        self.vm = new Vue({
            el: '#confirm-wrapper',
            data: {
                isVisible: false,
                title: '',
                okButton: 'OK',
                cancelButton: 'Cancel',
                okCallBack: null,
                cancelCallBack: null,
                header: ''
            },
            methods: {
                okHandler: self.okHandler,
                cancelHandler: self.cancelHandler
            }
        });
    }

    self.show = function (params) {
        self.vm.title = params.title;
        self.vm.okButton = params.okButton || 'OK';
        self.vm.cancelButton = params.cancelButton || 'Cancel';
        self.vm.okCallBack = params.okCallBack;
        self.vm.cancelCallBack = params.cancelCallBack;
        self.vm.isVisible = true;
        self.vm.header = params.header;
        self.vm.isCentered = params.isCentered || false;
    }

    self.okHandler = function () {
        var vm = self.vm;

        vm.isVisible = false;

        if (vm.okCallBack && typeof vm.okCallBack === "function") {
            vm.okCallBack();
        }
    };

    self.cancelHandler = function () {
        var vm = self.vm;

        vm.isVisible = false;

        if (vm.cancelCallBack && typeof vm.cancelCallBack === "function") {
            vm.cancelCallBack();
        }
    };

    self.init();

    return self.show;
}());