(function () {
    cevsis.Notify = cevsis.Notify || {};
    cevsis.Notify.Info = function (content) {
        notify(content, "info", "Information");
    }
    cevsis.Notify.Success = function (content) {
        notify(content, "success", "Success");
    }
    cevsis.Notify.Warning = function (content) {
        notify(content, "warning", "Warning");
    }
    cevsis.Notify.Error = function (content) {
        notify(content, "danger", "Error");
    }

    function notify(c, t, ti) {
        toastr[t](c, ti);
    }

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "7000",
        "extendedTimeOut": "3000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
})();