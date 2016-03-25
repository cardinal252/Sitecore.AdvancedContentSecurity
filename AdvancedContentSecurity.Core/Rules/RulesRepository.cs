using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace AdvancedContentSecurity.Core.Rules
{
    [ExcludeFromCodeCoverage] // Repository class - integration tests only
    public class RulesRepository : IRulesRepository
    {
        public bool Evaluate<T>(T ruleContext, IEnumerable<Rule<T>> rulesList) where T : RuleContext
        {
            bool result = true;
            if (rulesList == null)
            {
                return true;
            }

            foreach (Rule<T> rule in rulesList)
            {
                if (rule.Condition == null)
                {
                    continue;
                }

                result = EvaluateCondition(ruleContext, rule);
            }

            return result;
        }

        public void Execute<T>(T ruleContext, IEnumerable<Rule<T>> rulesList) where T : RuleContext
        {
            if (rulesList == null)
            {
                return;
            }

            foreach (Rule<T> rule in rulesList)
            {
                if (rule.Condition == null || !EvaluateCondition(ruleContext, rule))
                {
                    continue;
                }

                foreach (var action in rule.Actions)
                {
                    if (ruleContext.IsAborted)
                    {
                        continue;
                    }

                    action.Apply(ruleContext);
                }
            }
        }

        [ExcludeFromCodeCoverage]
        public IEnumerable<Rule<T>> GetRules<T>(string fieldName, Item item) where T : RuleContext
        {
            if (String.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            return !String.IsNullOrWhiteSpace(item[fieldName]) 
                ? RuleFactory.GetRules<T>(item.Fields[fieldName]).Rules 
                : Enumerable.Empty<Rule<T>>();
        }

        private bool EvaluateCondition<T>(T ruleContext, Rule<T> rule) where T : RuleContext
        {
            RuleStack stack = new RuleStack();
            rule.Condition.Evaluate(ruleContext, stack);
            if (ruleContext.IsAborted || stack.Count == 0)
            {
                return true;
            }

            return (bool)stack.Pop();
        }
    }
}
