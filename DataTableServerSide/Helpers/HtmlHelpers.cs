﻿using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataTableServerSide.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTableServerSide.Helpers
{
    public static class HtmlHelpers
    {
        public static HtmlString DataTable<T>(this IHtmlHelper<T> helper, ViewConfiguration config, IEnumerable<object> model)
        {
            var type = helper.GetType().GetGenericArguments().First();
            var vm = ArrangeFields(type, config);
            var headers = vm.Models.Where(i => !i.IsHidden).Select(i => i.DisplayName).ToList();
            var headerColumns = new List<string>();
            foreach (var header in headers)
            {
                headerColumns.Add(string.Format(HtmlStrings.TableStrings.HeaderColumnContent, header));
            }
            headerColumns.Add(string.Format(HtmlStrings.TableStrings.HeaderColumnContent, ""));
            var headerRow = string.Format(HtmlStrings.TableStrings.RowContent, string.Join(Environment.NewLine, headerColumns));
            var headerHtml = string.Format(HtmlStrings.TableStrings.TableHead, headerRow);

            
            var table = string.Format(HtmlStrings.TableStrings.Table, config.TableName, headerHtml, HtmlStrings.TableStrings.TableBody);
            return new HtmlString(table.ToString());
        }
        public static HtmlString ViewInitializer<T>(this IHtmlHelper<T> helper, ViewConfiguration config, IEnumerable<object> model = null)
        {
            var type = helper.GetType().GetGenericArguments().First();
            var vm = ArrangeFields(type, config);
            var settings = new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeHtml };
            return new HtmlString(JsonConvert.SerializeObject(vm, Formatting.None, settings));
        }

        private static ClientSideViewModel ArrangeFields(Type type, ViewConfiguration config)
        {
            var vm = new ClientSideViewModel()
            {
                ContainerId = config.ContainerName,
                FormId = config.FormId,
                TableName = config.TableName,
                GetAddress = config.GetAddress,
            };
            var subType = type.GetGenericArguments().First();
            var properties = subType.GetProperties();
            foreach (var property in properties)
            {
                var csm = new ClientSideModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = property.Name.Substring(0, 1).ToLower().Replace("ı", "i") + property.Name.Substring(1)
                };
                var isRequired = property.GetCustomAttribute<RequiredAttribute>(true);
                var isHidden = property.GetCustomAttribute<HiddenColumnAttribute>(true);
                var displayName = property.GetCustomAttribute<DisplayAttribute>(true);
                csm.IsRequired = isRequired != null;
                csm.IsHidden = isHidden != null;
                csm.DisplayName = displayName != null ? displayName.Name : property.Name;
                vm.Models.Add(csm);
            }

            return vm;
        }
    }

    public struct HtmlStrings
    {
        public struct TableStrings
        {
            public const string Table = "<table id='{0}' class='table table-bordered table-hover table-condensed table-striped table-responsive data-table compact'>{1} {2}</table>";
            public const string TableHead = "<thead>{0}</thead>";
            public const string TableBody = "<tbody></tbody>";
            public const string RowContent = "<tr>{0}</tr>";
            public const string HeaderColumnContent = "<th>{0}</th>";
            public const string LastHeaderColumnContent = "<th width='100'></th>";
        }
    }
}
