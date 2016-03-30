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
                        return Sitecore.Configuration.Settings.GetSetting("AdvancedContentSecurity.ConfigurationItem");
                    }
                }
            }

            public static class Devices
            {
                public static string RestrictedDeviceId
                {
                    get
                    {
                        return Sitecore.Configuration.Settings.GetSetting("AdvancedContentSecurity.RestrictedDeviceId");
                    }
                }
            }
        }
    }
}
