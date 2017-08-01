(function () {
    cevsis.Binding = cevsis.Binding || {};
    cevsis.Binding.initialize = function (json) {
        var model = JSON.parse(json);

        var viewModel = function () {
            var self = this;
            self.validator = {};
            self.validationRules = {};
            self.validationMessages = {};
            self.entities = [];
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
                self.validator.resetForm();
                $("select.selectComp").val(null).trigger("change");
                $("#" + model.ContainerId + " .errorCount").hide();
                self.showTable();
                self.selected(self.EmptyProperties);
            }
            self.saveRecord = function () {
                var tabs = $('#' + model.FormTab + ' li a');
                tabs.each(function (key, ele) {
                    validateTab(ele);
                });

                if ($("#" + model.FormId).valid()) {
                    setDefaults(self.selected());
                    var obj = JSON.stringify(ko.toJS(self.selected()));
                    $.ajax({
                        url: model.SaveAction,
                        type: "POST",
                        data: obj,
                        contentType: "application/json"
                    }).done(function (data) {
                        self.showTable();
                        self.dataTable.draw();
                        self.afterSuccess(data);
                    }).fail(function (err) {
                        self.showTable();
                        self.afterFail(err);
                    });
                }
            }
            self.afterSuccess = function (data) {

            }
            self.afterFail = function (data) {

            }
            self.mapFields = function (data) {
                var selected = {};
                for (var property in data) {
                    if (data.hasOwnProperty(property)) {
                        selected[property] = ko.observable(data[property]);
                        var selectComp = cevsis.utils.selectControls.filter(function (i) {
                            return i.name == property;
                        });
                        if (selectComp && selectComp.length > 0) {
                            for (var sc in selectComp) {
                                if (data[property].constructor === Array) {
                                    cevsis.utils.selectItem(selectComp[sc].name, data[property]);
                                }
                                else {
                                    var arr = [data[property]];
                                    cevsis.utils.selectItem(selectComp[sc].name, arr);
                                }
                            }
                        }
                    }
                }
                self.selected(selected);
            }
            function mapObservables(models) {
                self.EmptyProperties = {};
                for (var i = 0; i < models.length; i++) {
                    var entity = models[i];
                    self.EmptyProperties[entity.Name] = ko.observable("");
                    self.validationRules[entity.Name] =
                        {
                            required: false,
                            number: false
                        };
                    if (entity.IsRequired)
                        self.validationRules[entity.Name].required = true;
                    if (entity.IsNumber)
                        self.validationRules[entity.Name].number = true;
                    self.entities.push(entity);
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
                if (length == 0)
                    tabLink.hide();
                else
                    tabLink.show();
            };

            function setDefaults(model) {
                if (model.isNew()) {
                    model.id(0);
                }
                for (var property in self.entities) {
                    var prop = self.entities[property];
                    var p = model[prop.Name];
                    if (p() == null) {
                        if (prop.IsNumber)
                            p(0);
                        else if (prop.IsText)
                            p("");
                        else if (prop.IsArray)
                            p([]);
                    }
                }
            }
        }

        var vm = new viewModel();
        ko.applyBindings(vm, document.getElementById(model.ContainerId));
        window.onload = function () {
            $(".errorCount").hide();
            $("#" + model.TableName).ToTrDataTable(model, vm);

            for (var m in model.Models) {
                var mdl = model.Models[m];
                if (!mdl.AutoCompleteSource)
                    continue;
                $("select[name='" + mdl.Name + "'].selectComp").ToTrSelect(mdl.AutoCompleteSource);
                cevsis.utils.selectControls.push({ control: mdl.AutoCompleteSource, name: mdl.Name, value: "" });
            }

            vm.validator = $('#' + model.FormId).validate({
                lang: "tr",
                rules: vm.validationRules,
                ignore: []
            });
        }
        return vm;
    }
})();