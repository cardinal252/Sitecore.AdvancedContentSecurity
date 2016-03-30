using System;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Rules;
using AdvancedContentSecurity.Core.UserSecurity;

namespace AdvancedContentSecurity.Core.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        public ConfigurationFactory() : this(
                () => new ItemSecurityManager(new ItemSecurityRepository()),
                () => new RulesManager(new RulesRepository()),
                () => new ItemRepository(),
                x => new ContentSecurityManager(x.GetItemSecurityManager(), x.GetRulesManager(), x.GetItemRepository()),
                x => new UserSecurityManager(x.GetRulesManager(), x.GetItemRepository()),
                new TracerRepository())
        {
        }

        public ConfigurationFactory(Func<IItemSecurityManager> itemSecurityManagerFunc, Func<IRulesManager> rulesManagerFunc, Func<IItemRepository> itemRepositoryFunc, Func<IConfigurationFactory, IContentSecurityManager> contentSecurityManagerFunc, Func<IConfigurationFactory, IUserSecurityManager> userSecurityManagerFunc, ITracerRepository tracerRepository)
        {
            ItemSecurityManagerFunc = itemSecurityManagerFunc;
            RulesManagerFunc = rulesManagerFunc;
            ContentSecurityManagerFunc = contentSecurityManagerFunc;
            ItemRepositoryFunc = itemRepositoryFunc;
            UserSecurityManagerFunc = userSecurityManagerFunc;
            TracerRepository = tracerRepository;
        }

        protected Func<IItemSecurityManager> ItemSecurityManagerFunc { get; set; }

        protected Func<IConfigurationFactory, IContentSecurityManager> ContentSecurityManagerFunc { get; set; }

        protected Func<IRulesManager> RulesManagerFunc { get; set; }

        protected Func<IItemRepository> ItemRepositoryFunc { get; set; }

        protected Func<IConfigurationFactory, IUserSecurityManager> UserSecurityManagerFunc { get; set; }

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

        public IItemRepository GetItemRepository()
        {
            return ItemRepositoryFunc();
        }

        public IUserSecurityManager GetUserSecurityManager()
        {
            return UserSecurityManagerFunc(this);
        }

        public ITracerRepository TracerRepository { get; set; }

    }

}
