toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "rtl": true,
    "positionClass": "toast-bottom-left",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": 300,
    "hideDuration": 500,
    "timeOut": 3000,
    "extendedTimeOut": 1000,
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

function ToastSuccess(message, title) {
    toastr.success(message, title);
}

function ToastInfo(message, title) {
    toastr.info(message, title);
}

function ToastWarning(message, title) {
    toastr.warning(message, title);
}

function ToastError(message, title) {
    toastr.error(message, title);
}