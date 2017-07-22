var cevsis = cevsis || {};
cevsis.binding = cevsis.binding || {};
cevsis.binding.initialize = function (json) {
    var model = JSON.parse(json);
    
    var viewModel = function () {
        var self = this;
        self.panelHeader = ko.observable("");
        self.selected = ko.observable({});
        self.tableVisible = ko.observable(true);
        self.detailVisible = ko.observable(false);
        self.inputVisible = ko.observable(false);
        self.headerClass = ko.observable("panel-info");
        self.EmptyProperties = {};
        mapObservables(model.Models);

        self.showInputArea = function () {
            self.tableVisible(false);
            self.detailVisible(false);
            self.inputVisible(true);
            self.panelHeader("Yeni Kayıt");
        }
        self.showTable = function () {
            self.tableVisible(true);
            self.detailVisible(false);
            self.inputVisible(false);
        }

        self.selectRecord = function () {
            self.detailVisible(true);
            self.tableVisible(false);
            self.panelHeader("Kayıt Detayı");
            self.afterSuccess("select");
        }

        self.newRecord = function () {
            self.showInputArea();
            self.selected(self.EmptyProperties);
            self.headerClass("panel-success");
            self.afterSuccess("new");
        }

        self.updateRecord = function () {
            self.showInputArea();
            self.panelHeader("Kayıt Düzenle");
            self.headerClass("panel-warning");
        }

        self.cancelRecord = function () {
            self.showTable();
            self.selected(self.EmptyProperties);
        }

        self.afterSuccess = function (data) {

        }
        self.mapFields = function (data) {
            var selected = {};
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    selected[property] = ko.observable(data[property]);
                }
            }
            self.selected(selected);
        }
        function mapObservables(models) {
            self.EmptyProperties = {};
            for (var i = 0; i < models.length; i++) {
                self.EmptyProperties[models[i].Name] = ko.observable("");
            }
            self.selected(self.EmptyProperties);
        }
    }

    var vm = new viewModel();
    ko.applyBindings(vm, document.getElementById(model.ContainerId));
    window.onload = function () {
        $("#" + model.TableName).ToTrDataTable(model, vm);
    }
    return vm;
}
