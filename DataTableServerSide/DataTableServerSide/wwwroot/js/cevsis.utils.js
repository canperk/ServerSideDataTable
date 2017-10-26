(function () {
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
            for (var i = 0; i < data.results.length; i++) {
                var sItem = data.results[i];
                $("select[name='" + ctrl.name + "']").select2("trigger", "select", {
                    data: { id: sItem.id, text: sItem.text }
                });
            }

        }).fail(function (error) {
            cevsis.Notify.Error("Veri okunamadı");
        });

    }
})();