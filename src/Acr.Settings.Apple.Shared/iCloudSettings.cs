using System;
using Foundation;


namespace Acr.Settings
{
    public class iCloudSettings : AbstractSettings
    {
        protected NSUbiquitousKeyValueStore Store => NSUbiquitousKeyValueStore.DefaultStore;


        public override bool Contains(string key)
        {
            return (this.Store.ValueForKey(new NSString(key)) != null);
        }


        protected override void NativeSet(Type type, string key, object value)
        {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {

                case TypeCode.Boolean:
                    this.Store.SetBool(key, (bool)value);
                    break;

                case TypeCode.Double:
                    this.Store.SetDouble(key, (double)value);
                    break;

                case TypeCode.Int64:
                    this.Store.SetLong(key, (long)value);
                    break;

                case TypeCode.String:
                    this.Store.SetString(key, (string)value);
                    break;

                default:
                    var @string = this.Serialize(type, value);
                    this.Store.SetString(key, @string);
                    break;
            }

            this.Store.Synchronize();
        }


        protected override object NativeGet(Type type, string key)
        {
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {

                case TypeCode.Boolean:
                    return this.Store.GetBool(key);

                case TypeCode.Double:
                    return this.Store.GetDouble(key);

                case TypeCode.Int64:
                    return this.Store.GetLong(key);

                case TypeCode.String:
                    return this.Store.GetString(key);

                default:
                    var @string = this.Store.GetString(key);
                    return this.Deserialize(type, @string);
            }
        }


        protected override void NativeRemove(string[] keys)
        {
            foreach (var key in keys)
                this.Store.Remove(key);

            this.Store.Synchronize();
        }
    }
}