using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace ContentSecurity.Core.Rules
{
    public interface IRulesRepository
    {
        bool Evaluate<T>(T ruleContext, IEnumerable<Rule<T>> rulesList) where T : RuleContext;

        void Execute<T>(T ruleContext, IEnumerable<Rule<T>> rulesList) where T : RuleContext;

        IEnumerable<Rule<T>> GetRules<T>(string fieldName, Item item) where T : RuleContext;
    }
}
