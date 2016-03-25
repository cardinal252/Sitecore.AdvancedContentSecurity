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

            public static string RulesRestricted
            {
                get { return "item:rulesrestricted"; }
            }

            public static string RulesRead
            {
                get { return "item:rulesread"; }
            }
        }
    }
}
