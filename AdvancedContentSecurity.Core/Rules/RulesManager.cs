using System;
using System.Collections.Generic;
using System.Linq;
using ContentSecurity.Core.Rules;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace AdvancedContentSecurity.Core.Rules
{
    public class RulesManager : IRulesManager
    {
        public RulesManager(IRulesRepository rulesRepository)
        {
            RulesRepository = rulesRepository;
        }

        protected IRulesRepository RulesRepository { get; private set; }

        public virtual bool EvaluateRulesFromField<T>(T ruleContext, string fieldName, Item rulesItem) where T : RuleContext
        {
            if (rulesItem == null)
            {
                throw new ArgumentNullException(nameof(rulesItem));
            }

            try
            {

                IEnumerable<Rule<RuleContext>> rules = RulesRepository.GetRules<RuleContext>(fieldName, rulesItem);

                if (rules == null)
                {
                    return true;
                }

                Rule<RuleContext>[] rulesList = rules.ToArray();

                return RulesRepository.Evaluate(ruleContext, rulesList);
            }
            catch (Exception ex)
            {
                // todo: do something with the exception
                return false;
            }
        }

        public virtual bool EvaluateRulesFromField<T>(string fieldName, Item rulesItem, Item itemToEvaluate) where T : RuleContext, new()
        {
            return EvaluateRulesFromField(new T { Item = itemToEvaluate }, fieldName, rulesItem);
        }

        public virtual bool ExecuteRulesFromField<T>(T ruleContext, string fieldName, Item rulesItem) where T : RuleContext
        {
            if (rulesItem == null)
            {
                throw new ArgumentNullException(nameof(rulesItem));
            }

            try
            {
                IEnumerable<Rule<RuleContext>> rules = RulesRepository.GetRules<RuleContext>(fieldName, rulesItem);

                if (rules == null)
                {
                    return true;
                }

                Rule<RuleContext>[] rulesList = rules.ToArray();
                RulesRepository.Execute(ruleContext, rulesList);
                return true;
            }
            catch (Exception ex)
            {
                // todo: do something with the exception
                return false;
            }
        }

        public virtual bool ExecuteRulesFromField<T>(string fieldName, Item rulesItem, Item itemToEvaluate) where T : RuleContext, new()
        {
            return ExecuteRulesFromField(new T { Item = itemToEvaluate}, fieldName, rulesItem);
        }
    }
}
