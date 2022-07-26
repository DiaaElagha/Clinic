function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}

$(document).on('submit', '.ajaxForm', function (e) {
    e.preventDefault();

    let _url = $(this).attr('action');
    console.log(_url);
    $.ajax({
        type: 'POST',
        url: _url,
        data: $(this).serialize(),
        success: function (json) {
            if (!$(this).isValid) {
                $('#PopUp .modal-body').html(json);
            }
            else if (json.status == 1) {
                var tname = $(this).attr("tname");
                var fname = $(this).attr("fname");
                if (tname != null) {
                    RefreshData(tname);
                }
                if (fname != null) {
                    eval(fname);
                }
                if (!$("#tblItems").hasClass("autohide")) {
                    $(this).resetForm();
                    $("#tblItems tbody tr").remove();
                    $("#tblItems").addClass("hidden").next().show();
                }
                else {
                    $(".select2").val('').change();
                }
            }
            console.log(json.close);
            if (json.msg != null)
                ShowMessage(json.msg);
            if (json.redirect != null)
                window.location = json.redirect;
            if (json.close == 1)
                $("#PopUp").modal("hide");
            $(".ajaxForm :submit").prop("disabled", false);
        },
        beforeSubmit: function () {
            $(".ajaxForm :submit").prop("disabled", true);
        },
        error: function (err) {
            console.log(err)
        }
    });

    return false;
});

$(document).on("click", ".PopUp", function () {
    $("#PopUp").modal("show");
    $("#PopUp .modal-title").text($(this).attr("data-title"));
    $("#PopUp .modal-body").html('<h3 class="text-center text-danger"><i class="fa fa-spinner fa-spin fa-2x fa-fw"></i></h3>');
    $("#PopUp .modal-body").load($(this).attr("data-link"), function () {
        setTimeout(function () { $("#PopUp [autofocus]").focus().select(); }, 500);
    });
    return false;
});

$(document).on("click", ".Confirm", function () {
    $("#Confirm").modal("show");
    $("#Confirm .btn-danger").attr("data-link", $(this).attr("data-link"));
    $("#Confirm .btn-danger").attr("tname", $(this).attr("tname"));
    return false;
});

$("#Confirm .btn-danger").click(function () {
    var tname = $(this).attr("tname");
    var url = $(this).attr("data-link");
    console.log(url);
    $.ajax({
        url: url,
        success: function (json) {
            if (json.status == 1) {
                if (tname != null) {
                    RefreshData(tname);
                }
            }
            ShowMessage(json.msg);
        }
    });
    $("#Confirm").modal("hide");
    return false;
});

function RefreshData(tn) {
    var xtable = $(tn).dataTable();
    var info = $(tn).DataTable().page.info();
    xtable.fnDraw();
    xtable.fnPageChange(info.page);
}

$(document).ajaxStart(function () {
    $(".wait").text("Wait...");
    NProgress.start();
});
$(document).ajaxStop(function () {
    $(".wait").text("");
    NProgress.done();
});
$(document).ajaxError(function () {
    $(".wait").text("");
    NProgress.done();
    $(".ajaxForm :submit").prop("disabled", false);
});

toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

function ShowMessage(msg) {
    console.log(msg);
    var cls = "info";
    if (msg.indexOf("s:") == 0) { cls = "success"; msg = msg.substring(2); }
    if (msg.indexOf("w:") == 0) { cls = "warning"; msg = msg.substring(2); }
    if (msg.indexOf("e:") == 0) { cls = "error"; msg = msg.substring(2); }
    if (msg.indexOf("i:") == 0) { cls = "info"; msg = msg.substring(2); }
    toastr[cls](msg);
}

