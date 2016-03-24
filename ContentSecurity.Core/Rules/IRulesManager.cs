using Sitecore.Data.Items;
using Sitecore.Rules;

namespace ContentSecurity.Core.Rules
{
    public interface IRulesManager
    {
        bool EvaluateRulesFromField<T>(T ruleContext, string fieldName, Item rulesItem) where T : RuleContext;

        bool EvaluateRulesFromField<T>(string fieldName, Item rulesItem, Item itemToEvaluate) where T : RuleContext, new();

        bool ExecuteRulesFromField<T>(T ruleContext, string fieldName, Item rulesItem) where T : RuleContext;

        bool ExecuteRulesFromField<T>(string fieldName, Item rulesItem, Item itemToEvaluate) where T : RuleContext, new();
    }
}
