﻿using System.Diagnostics.CodeAnalysis;
using AdvancedContentSecurity.Core.ItemSecurity;
using ContentSecurity.Core;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace AdvancedContentSecurity.Core.Rules
{
    [ExcludeFromCodeCoverage] // This doesn't need testing since it contains no logic that is untested
    public class IsRestrictedCondition<T> : WhenCondition<T> where T : RuleContext
    {
        protected IItemSecurityManager ItemSecurityManager { get; private set; }

        public IsRestrictedCondition() : this(new ItemSecurityManager(new ItemSecurityRepository()))
        {
            
        }

        public IsRestrictedCondition(IItemSecurityManager itemSecurityManager)
        {
            ItemSecurityManager = itemSecurityManager;
        }

        protected override bool Execute(T ruleContext)
        {
            return ItemSecurityManager.HasPermission(ContentSecurityConstants.AccessRights.Restricted, ruleContext.Item, Sitecore.Context.User);
        }
    }
}