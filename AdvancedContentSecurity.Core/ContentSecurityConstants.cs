using System;
using Sitecore;
using Sitecore.Syndication;

namespace AdvancedContentSecurity.Core
{
    public static class ContentSecurityConstants
    {
        public static class AccessRights
        {
            public static string Restricted
            {
                get { return "item:restricted"; }
            }

            public static string Rules
            {
                get { return "item:rules"; }
            }
        }

        public static class FieldNames 
        {
            public static string ReadRules
            {
                get { return "Read Rules"; }
            }

            public static string RestrictedRules
            {
                get { return "Restricted Rules"; }
            }

            public static string Rule
            {
                get { return "Rule"; }
            }

            public static string UserInitialisationRule
            {
                get { return "User Initialisation Rule"; }
            }
        }

        public static class Ids
        {
            public static class Settings
            {
                public static string ConfigurationItemId
                {
                    get
                    {
                        string result = Sitecore.Configuration.Settings.GetSetting("AdvancedContentSecurity.ConfigurationItem");
                        return !String.IsNullOrEmpty(result) ? result : "{8B9F6F25-56F2-4A93-8C97-A0B63F54567A}";
                    }
                }
            }

            public static class Devices
            {
                public static string RestrictedDeviceId
                {
                    get
                    {
                        string result = Sitecore.Configuration.Settings.GetSetting("AdvancedContentSecurity.RestrictedDeviceId");
                        return !String.IsNullOrEmpty(result) ? result : "{73E718E6-70B4-4426-B94F-F705AB078448}";
                    }
                }
            }
        }
    }
}
