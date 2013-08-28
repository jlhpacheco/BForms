﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace BootstrapForms.Models
{
    /// <summary>
    /// Represents a list of items for Dropdown, ListBox, RadioList, CheckboxList binding
    /// </summary>
    public class BsSelectList<T>
    {
        private T selectedValues;

        /// <summary>
        /// Selected values
        /// </summary>
        public T SelectedValues
        {
            get
            {
                return selectedValues;
            }
            set
            {
                selectedValues = value;
            }
        }

        public static implicit operator T(BsSelectList<T> value)
        {
            return value.SelectedValues;
        }

        public static implicit operator BsSelectList<T>(T value)
        {
            return new BsSelectList<T> { SelectedValues = value };
        }

        public List<BsSelectListItem> Items { get; set; }

        public BsSelectList()
        {
            Items = new List<BsSelectListItem>();
        }

        /// <summary>
        /// Returns all select list items as IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> ToSelectList()
        {
            var list = new List<SelectListItem>();
            
            foreach (var item in Items)
            {
                list.Add(new SelectListItem
                         {
                             Selected = item.Selected,
                             Text = item.Text,
                             Value = item.Value
                         });
            }
            return list;
        }

        /// <summary>
        /// Returns a BsSelectList from 
        /// </summary>
        public static BsSelectList<T> FromSelectList(IEnumerable<SelectListItem> list)
        {
            var bsList = new BsSelectList<T>();
            foreach (var item in list)
            {
                bsList.Items.Add(new BsSelectListItem
                                 {
                                     Selected = item.Selected,
                                     Text = item.Text,
                                     Value = item.Value
                                 });
            }
            return bsList;
        }

        /// <summary>
        /// Returns a BsSelectList from enum using the DescriptionAttribute
        /// </summary>
        public static BsSelectList<T> FromEnum(Type myEnum)
        {
            var enumType = myEnum;
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("E is not of type enum", "myEnum");
            }

            var bsList = new BsSelectList<T>();
            foreach (var item in Enum.GetValues(enumType))
            {
                //get Description Name from resources
                var name = Enum.GetName(enumType, item);
                var text = enumType.GetMember(name)
                    .First()
                    .GetCustomAttributes(false)
                    .OfType<DescriptionAttribute>()
                    .LastOrDefault();

                var textValue = text == null ? name : text.Description;

                bsList.Items.Add(new BsSelectListItem
                                 {
                                     Selected = false,
                                     Text = textValue,
                                     Value = Convert.ChangeType(item, Enum.GetUnderlyingType(myEnum)).ToString()
                                 });
            }

            return bsList;
        }

    }
}