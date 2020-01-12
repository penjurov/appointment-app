var customAlert = (function () {
    var self = {};

    self.vm = {};

    self.init = function () {
        self.vm = new Vue({
            el: '#alert-wrapper',
            data: {
                isVisible: false,
                title: '',
                body: '',
                callback: null
            },
            methods: {
                onOkClicked: function () {
                    this.isVisible = false;
                    if (this.callback && typeof (this.callback) === "function") {
                        this.callback();
                    }
                }
            }
        });
    }

    self.show = function (title, body, callback) {
        self.vm.title = title;
        self.vm.body = body;
        self.vm.isVisible = true;
        self.vm.callback = callback;
    }

    self.init();

    return self.show;
}());