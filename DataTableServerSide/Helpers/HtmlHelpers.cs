using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataTableServerSide.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace DataTableServerSide.Helpers
{
    public static class HtmlHelpers
    {
        public static HtmlString DataTable<T>(this IHtmlHelper<T> helper, ViewConfiguration config)
        {
            var type = helper.GetType().GetGenericArguments().First();
            var vm = ArrangeFields(type, config);
            var headers = vm.Models.Where(i => !i.IsHidden).Select(i => i.DisplayName).ToList();
            var headerColumns = new List<string>();
            foreach (var header in headers)
            {
                headerColumns.Add(string.Format(HtmlStrings.TableStrings.HEADERCOLUMNCONTENT, header));
            }
            headerColumns.Add(string.Format(HtmlStrings.TableStrings.HEADERCOLUMNCONTENT, ""));
            var headerRow = string.Format(HtmlStrings.TableStrings.ROWCONTENT, string.Join(Environment.NewLine, headerColumns));
            var headerHtml = string.Format(HtmlStrings.TableStrings.TABLEHEAD, headerRow);


            var table = string.Format(HtmlStrings.TableStrings.TABLE, config.TableName, headerHtml, HtmlStrings.TableStrings.TABLEBODY);
            return new HtmlString(table.ToString());
        }
        public static HtmlString ViewInitializer<T>(this IHtmlHelper<T> helper, ViewConfiguration config)
        {
            var type = helper.GetType().GetGenericArguments().First();
            var vm = ArrangeFields(type, config);
            var settings = new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeHtml };
            return new HtmlString(JsonConvert.SerializeObject(vm, Formatting.None, settings));
        }
        public static HtmlString SelectControl<TModel, TResult>(this IHtmlHelper<TModel> helper, string label, Expression<Func<TModel, TResult>> expression)
        {
            var property = (expression.Body as MemberExpression).Member.Name;
            var name = property.Substring(0, 1).ToLower().Replace("ı", "i") + property.Substring(1);
            return new HtmlString(string.Format(HtmlStrings.SelectStrings.BODY, label, name));
        }
        public static HtmlString InputFormControl(this IHtmlHelper helper, string label, string property, bool isreadonly = false)
        {
            var readOnly = isreadonly ? "readonly='readonly'" : string.Empty;
            var html = $@"<div class='form-group'>
                            <label>{label}</label>
                            <input class='form-control' name='{property}' {readOnly} autocomplete='off' data-bind='value:selected().{property}' />
                        </div>";
            return new HtmlString(html);
        }
        public static HtmlString UpdatePanelFooter(this IHtmlHelper helper)
        {
            var html = @"<div class='panel-footer'>
                                <button class='btn btn-primary pull-right' data-bind='click:updateRecord'>Güncelle</button>
                                <button class='btn btn-danger pull-right' data-bind='click:cancelRecord'>İptal</button>
                                <div class='clearfix'></div>
                         </div>";
            return new HtmlString(html);
        }
        public static HtmlString NewPanelFooter(this IHtmlHelper helper)
        {
            var html = @"<div class='panel-footer'>
                                <button class='btn btn-success pull-right' data-bind='click:saveRecord'>Kaydet</button>
                                <button class='btn btn-danger pull-right' data-bind='click:cancelRecord'>İptal</button>
                                <div class='clearfix'></div>
                         </div>";
            return new HtmlString(html);
        }
        public static HtmlString NewRecordButton(this IHtmlHelper helper)
        {
            var html = @"<button class='btn btn-success pull-right' data-bind='click: newRecord'>Yeni</button>
                         <div class='clearfix'></div>";
            return new HtmlString(html);
        }
        public static HtmlString TabButton (this IHtmlHelper helper, string id, string text)
        {
            var html = $"<a data-target='#{id}' data-toggle='tab'>{text}<div class='errorCount'></div></a>";
            return new HtmlString(html);
        }
        private static ClientSideViewModel ArrangeFields(Type type, ViewConfiguration config)
        {
            var vm = new ClientSideViewModel()
            {
                ContainerId = config.ContainerName,
                FormId = config.FormId,
                TableName = config.TableName,
                GetAddress = config.GetAddress,
                SaveAction = config.SaveAction,
                FormTab = config.InputTabName
            };
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var csm = new ClientSideModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = property.Name.Substring(0, 1).ToLower().Replace("ı", "i") + property.Name.Substring(1),
                    FullName = property.Name
                };
                var isRequired = property.GetCustomAttribute<RequiredAttribute>(true);
                var isHidden = property.GetCustomAttribute<HiddenColumnAttribute>(true);
                var displayName = property.GetCustomAttribute<DisplayAttribute>(true);
                var order = property.GetCustomAttribute<OrderableAttribute>(true);
                var autoComplete = property.GetCustomAttribute<AutoCompleteAttribute>(true);
                csm.IsArray = property.PropertyType.IsArray;
                csm.IsNumber = property.PropertyType == typeof(int) || property.PropertyType == typeof(long) || property.PropertyType == typeof(short);
                csm.IsText = property.PropertyType == typeof(string);
                csm.IsRequired = isRequired != null;
                csm.IsHidden = isHidden != null;
                csm.IsOrderable = order != null;
                csm.DisplayName = displayName != null ? displayName.Name : property.Name;
                if (autoComplete != null)
                {
                    csm.AutoCompleteSource = new ClientSideModel.AutoComplete()
                    {
                        PageSize = autoComplete.PageSize,
                        Url = autoComplete.Url,
                        InputLength = autoComplete.InputLength
                    };
                }
                vm.Models.Add(csm);
            }

            return vm;
        }
    }

    public struct HtmlStrings
    {
        public struct TableStrings
        {
            public const string TABLE = "<table id='{0}' class='table table-bordered table-hover table-condensed table-striped table-responsive data-table compact'>{1} {2}</table>";
            public const string TABLEHEAD = "<thead>{0}</thead>";
            public const string TABLEBODY = "<tbody></tbody>";
            public const string ROWCONTENT = "<tr>{0}</tr>";
            public const string HEADERCOLUMNCONTENT = "<th>{0}</th>";
        }

        public struct SelectStrings
        {
            public const string BODY = @"<div class='form-group'>
                                            <label>{0}</label>
                                            <select name='{1}' class='form-control selectComp'></select>
                                        </div>";
        }
    }
}
