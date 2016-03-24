using Sitecore.Data.Items;
using Sitecore.Rules;

namespace ContentSecurity.Core.Rules
{
    public interface IRulesManager
    {
        bool EvaluateRulesFromField<T>(T ruleContext, string fieldName, Item item) where T : RuleContext;

        bool EvaluateRulesFromField<T>(string fieldName, Item item) where T : RuleContext, new();

        bool ExecuteRulesFromField<T>(T ruleContext, string fieldName, Item item) where T : RuleContext;

        bool ExecuteRulesFromField<T>(string fieldName, Item item) where T : RuleContext, new();
    }
}
