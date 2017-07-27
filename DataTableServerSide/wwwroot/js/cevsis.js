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









