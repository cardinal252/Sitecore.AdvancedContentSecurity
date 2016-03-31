using System;
using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core.ContentSecurity;
using AdvancedContentSecurity.Core.Context;
using AdvancedContentSecurity.Core.Items;
using AdvancedContentSecurity.Core.ItemSecurity;
using AdvancedContentSecurity.Core.Logging;
using AdvancedContentSecurity.Core.Rules;
using AdvancedContentSecurity.Core.UserSecurity;

namespace AdvancedContentSecurity.Core.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        static ConfigurationFactory()
        {
            Default = new ConfigurationFactory();
        }

        public static IConfigurationFactory Default { get; set; }

        [ExcludeFromCodeCoverage] // Wrapped constructor
        public ConfigurationFactory() : this(
                () => new ItemSecurityManager(new ItemSecurityRepository()),
                () => new RulesManager(new RulesRepository()),
                () => new ItemRepository(),
                x => new ContentSecurityManager(x.GetItemSecurityManager(), x.GetRulesManager(), x.GetItemRepository()),
                x => new UserSecurityManager(x.GetRulesManager(), x.GetItemRepository()),
                new TracerRepository(),
                () => new SitecoreContextWrapper())
        {
        }

        public ConfigurationFactory(Func<IItemSecurityManager> itemSecurityManagerFunc, Func<IRulesManager> rulesManagerFunc, 
            Func<IItemRepository> itemRepositoryFunc, Func<IConfigurationFactory, IContentSecurityManager> contentSecurityManagerFunc, 
            Func<IConfigurationFactory, IUserSecurityManager> userSecurityManagerFunc, ITracerRepository tracerRepository,
            Func<ISitecoreContextWrapper> sitecoreContextWrapperFunc)
        {
            ItemSecurityManagerFunc = itemSecurityManagerFunc;
            RulesManagerFunc = rulesManagerFunc;
            ContentSecurityManagerFunc = contentSecurityManagerFunc;
            ItemRepositoryFunc = itemRepositoryFunc;
            UserSecurityManagerFunc = userSecurityManagerFunc;
            TracerRepository = tracerRepository;
            SitecoreContextWrapperFunc = sitecoreContextWrapperFunc;
        }

        protected Func<IItemSecurityManager> ItemSecurityManagerFunc { get; private set; }

        protected Func<IConfigurationFactory, IContentSecurityManager> ContentSecurityManagerFunc { get; private set; }

        protected Func<IRulesManager> RulesManagerFunc { get; private set; }

        protected Func<IItemRepository> ItemRepositoryFunc { get; private set; }

        protected Func<IConfigurationFactory, IUserSecurityManager> UserSecurityManagerFunc { get; private set; }

        protected Func<ISitecoreContextWrapper> SitecoreContextWrapperFunc { get; private set; } 

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

        public ISitecoreContextWrapper GetSitecoreContextWrapper()
        {
            return SitecoreContextWrapperFunc();
        }

        public ITracerRepository TracerRepository { get; set; }

    }

}
