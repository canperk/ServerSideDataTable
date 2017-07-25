﻿var cevsis = cevsis || {};
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
            for (var index in self.EmptyProperties) {
                if (ko.isObservable(self.EmptyProperties[index])) {
                    self.EmptyProperties[index](null);
                }
            }
            self.selected(self.EmptyProperties);
            self.headerClass("panel-success");
            self.selected().isNew = ko.observable(true);
        }

        self.updateRecord = function () {
            self.showInputArea();
            self.panelHeader("Kayıt Düzenle");
            self.headerClass("panel-warning");
            self.selected().isNew = ko.observable(false);
        }

        self.cancelRecord = function () {
            self.showTable();
            self.selected(self.EmptyProperties);
        }
        self.saveRecord = function () {
            
            var tabs = $('#tabList li a');
            tabs.each(function (key, ele) {
                validateTab(ele);
            });
            
            //if ($("#" + model.FormId).valid()) {
            //    var obj = ko.toJS(self.selected());
            //    console.log(obj);
            //    self.showTable();
            //    alert("Çalıştı");
            //}
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

        function validateTab(element) {
            var _element = $(element);
            var validatePane = _element.attr('data-target');
            var isValid = $(validatePane + ' :input').valid();
            var length = $(validatePane + ' input[aria-invalid="true"]').length;
            var tabLink = $("a[data-target='" + validatePane + "'] .errorCount");
            tabLink.text(length);
            if (length == 0) {
                tabLink.hide();
            }
            else {
                tabLink.show();
            }
        };
    }

    var vm = new viewModel();
    ko.applyBindings(vm, document.getElementById(model.ContainerId));
    window.onload = function () {
        $("#" + model.TableName).ToTrDataTable(model, vm);

        $('#' + model.FormId).validate({
            rules: {
                name: 'required',
                price: { required: true, number: true },
                stock: { required: true, number: true }
            },
            ignore: []
        });
    }
    return vm;
}
