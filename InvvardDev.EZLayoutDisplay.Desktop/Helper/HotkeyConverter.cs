﻿using System;
using System.ComponentModel;
using System.Globalization;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class HotkeyConverter : TypeConverter
    {
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public HotkeyConverter()
        {
            jsonSerializerSettings = new JsonSerializerSettings {
                                                                    ContractResolver = new CustomContractResolver()
                                                                };
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) { return true; }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (!(value is string)) return base.ConvertFrom(context, culture, value);

            var setting = value.ToString();

            var hotkey = !string.IsNullOrWhiteSpace(setting)
                             ? JsonConvert.DeserializeObject<Hotkey>(setting, jsonSerializerSettings)
                             : new Hotkey(0x60, ModifierKeys.Control, ModifierKeys.Alt);

            return hotkey;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string)) return base.ConvertTo(context, culture, value, destinationType);

            var hotkeySetting = JsonConvert.SerializeObject(value, jsonSerializerSettings);

            return hotkeySetting;
        }
    }

    internal class CustomContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = typeof(Hotkey).IsAssignableFrom(objectType)
                               ? CreateObjectContract(objectType)
                               : base.CreateContract(objectType);

            return contract;
        }
    }
}