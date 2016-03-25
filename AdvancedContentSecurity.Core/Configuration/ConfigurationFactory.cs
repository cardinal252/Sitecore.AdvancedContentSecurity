using System;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Rules;

namespace AdvancedContentSecurity.Core.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        public ConfigurationFactory(Func<IItemSecurityManager> itemSecurityManagerFunc, Func<IRulesManager> rulesManagerFunc, Func<IItemManager> itemManagerFunc, Func<IConfigurationFactory, IContentSecurityManager> contentSecurityManagerFunc)
        {
            ItemSecurityManagerFunc = itemSecurityManagerFunc;
            RulesManagerFunc = rulesManagerFunc;
            ContentSecurityManagerFunc = contentSecurityManagerFunc;
            ItemManagerFunc = itemManagerFunc;
        }

        protected Func<IItemSecurityManager> ItemSecurityManagerFunc { get; set; }

        protected Func<IConfigurationFactory, IContentSecurityManager> ContentSecurityManagerFunc { get; set; }

        protected Func<IRulesManager> RulesManagerFunc { get; set; }

        protected Func<IItemManager> ItemManagerFunc { get; set; } 

        public IContentSecurityManager GetContentSecurityManager()
        {
            return ContentSecurityManagerFunc(this);
        }

        public IItemSecurityManager GetItemSecurityManager()
        {
            return ItemSecurityManagerFunc();
        }

        public IRulesManager GetRulesManager()
        {
            return RulesManagerFunc();
        }

        public IItemManager GetItemManager()
        {
            return ItemManagerFunc();
        }

        public ITracerRepository TracerRepository { get; set; }

    }

}
