﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;


namespace Acr.Settings
{

    public class DefaultSettings : AbstractSettings
    {
        readonly ApplicationDataContainer container;


        public DefaultSettings(bool isRoaming = false)
        {
            this.IsRoamingProfile = isRoaming;
            this.container = this.IsRoamingProfile
                ? ApplicationData.Current.RoamingSettings
                : ApplicationData.Current.LocalSettings;
        }


        public override bool Contains(string key) => this.container.Values.ContainsKey(key);


        protected override object NativeGet(Type type, string key)
        {
            var @string = (string)this.container.Values[key];
            var @object = this.Deserialize(type, @string);
            return @object;
        }


        protected override void NativeSet(Type type, string key, object value)
        {
            var @string = this.Serialize(type, value);
            this.container.Values[key] = @string;
        }


        protected override void NativeRemove(string[] keys)
        {
            foreach (var key in keys)
                this.container.Values.Remove(key);
        }


        protected override IDictionary<string, string> NativeValues() =>
            this.container
                .Values
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.ToString()
                );
    }
}
