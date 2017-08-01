var cevsis = cevsis || {};

$.fn.ToTrDataTable = function (model, viewModel) {
    var columns = [];
    for (var i = 0; i < model.Models.length; i++) {
        var col = model.Models[i];
        if (!col.IsHidden) {
            var obj = { data: col.Name, name: col.FullName, orderable: col.IsOrderable };
            columns.push(obj);
        }
    }
    columns.push({ data: "", orderable: false });
    var table = $(this).DataTable({
        bFilter: false,
        destroy: true,
        bLengthChange: false,
        processing: true,
        pageLength: 15,
        order: [],
        serverSide: true,
        columns: columns,
        columnDefs: [{
            "targets": -1,
            "data": null,
            "defaultContent": "<button class='selectRow btn btn-info btn-sm'>Görüntüle</button>"
        }],
        ajax: {
            "type": "POST",
            "url": model.GetAddress,
            "contentType": 'application/json; charset=utf-8',
            'data': function (data) { return data = JSON.stringify(data); }
        },
        language: {
            "sDecimal": ",",
            "sEmptyTable": "Tabloda herhangi bir veri mevcut değil",
            "sInfo": "Listede <b>_START_ - _END_</b> arası kayıtlar listeleniyor. <b>Toplam : _TOTAL_ kayıt</b>",
            "sInfoEmpty": "Kayıt yok",
            "sInfoFiltered": "",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Sayfada _MENU_ kayıt göster",
            "sLoadingRecords": "Yükleniyor...",
            "sProcessing": "İşleniyor...",
            "sSearch": "Ara:",
            "sZeroRecords": "Eşleşen kayıt bulunamadı",
            "oPaginate": {
                "sFirst": "İlk",
                "sLast": "Son",
                "sNext": "Sonraki",
                "sPrevious": "Önceki"
            },
            "oAria": {
                "sSortAscending": ": artan sütun sıralamasını aktifleştir",
                "sSortDescending": ": azalan sütun sıralamasını aktifleştir"
            }
        }
    });
    $('#' + model.TableName + ' tbody').on('click', 'button.selectRow', function () {
        var data = table.row($(this).parents('tr')).data();
        vm.mapFields(data);
        vm.selectRecord();
    });
    viewModel.dataTable = table;
}
$.fn.ToTrSelect = function (model) {
    $(this).select2({
        language: "tr",
        placeholder: 'Seçiniz',
        minimumInputLength: model.InputLength,
        allowClear: true,
        theme: "bootstrap",
        ajax: {
            quietMillis: 300,
            url: model.Url,
            contentType: 'json',
            data: function (term, page) {
                return {
                    pageSize: model.PageSize,
                    searchTerm: term.term
                };
            },
            results: function (data, page) {
                var more = (page * model.PageSize) < data.Total;
                return { results: data.Results, more: more };
            }
        }
    });
}
cevsis.utils = cevsis.utils || {};
cevsis.utils.selectControls = [];
cevsis.utils.selectItem = function (name, ids) {
    var control = cevsis.utils.selectControls.filter(function (item) {
        return item.name == name;
    });
    if (control.length == 0)
        return;
    var ctrl = control[0];
    var parts = [];
    for (var i = 0; i < ids.length; ++i)
        parts.push(encodeURIComponent("values") + '=' + encodeURIComponent(ids[i]));
    var qs = parts.join('&');
    $.ajax({
        url: ctrl.control.Url + "?pageSize=15&" + qs,
        type: "POST",
        contentType: "application/json"
    }).done(function (data) {
        console.log(data);
        //$("select[name='" + control.control.Name + "']").select2("trigger", "select", {
        //    data: { id: "1", text: "Can" }
        //});
    }).fail(function (error) {
        cevsis.Notify.Error("Veri okunamadı");
    });

}

ko.bindingHandlers.slideIn = {
    init: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).toggle(value);
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        value ? $(element).slideDown(800, "easeOutBounce") : $(element).slideUp(800, "easeOutBounce");
    }
};
$.extend($.validator.messages, {
    required: "Bu alanın doldurulması zorunludur.",
    remote: "Lütfen bu alanı düzeltin.",
    email: "Lütfen geçerli bir e-posta adresi giriniz.",
    url: "Lütfen geçerli bir web adresi (URL) giriniz.",
    date: "Lütfen geçerli bir tarih giriniz.",
    dateISO: "Lütfen geçerli bir tarih giriniz(ISO formatında)",
    number: "Lütfen geçerli bir sayı giriniz.",
    digits: "Lütfen sadece sayısal karakterler giriniz.",
    creditcard: "Lütfen geçerli bir kredi kartı giriniz.",
    equalTo: "Lütfen aynı değeri tekrar giriniz.",
    extension: "Lütfen geçerli uzantıya sahip bir değer giriniz.",
    maxlength: $.validator.format("Lütfen en fazla {0} karakter uzunluğunda bir değer giriniz."),
    minlength: $.validator.format("Lütfen en az {0} karakter uzunluğunda bir değer giriniz."),
    rangelength: $.validator.format("Lütfen en az {0} ve en fazla {1} uzunluğunda bir değer giriniz."),
    range: $.validator.format("Lütfen {0} ile {1} arasında bir değer giriniz."),
    max: $.validator.format("Lütfen {0} değerine eşit ya da daha küçük bir değer giriniz."),
    min: $.validator.format("Lütfen {0} değerine eşit ya da daha büyük bir değer giriniz."),
    require_from_group: "Lütfen bu alanların en az {0} tanesini doldurunuz."
});
$.fn.select2.defaults.set("width", null);








