using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace ContentSecurity.Core.Rules
{
    public class RulesManager : IRulesManager
    {
        public RulesManager(IRulesRepository rulesRepository)
        {
            RulesRepository = rulesRepository;
        }

        protected IRulesRepository RulesRepository { get; private set; }

        public virtual bool EvaluateRulesFromField<T>(T ruleContext, string fieldName, Item item) where T : RuleContext
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            try
            {

                IEnumerable<Rule<RuleContext>> rules = RulesRepository.GetRules<RuleContext>(fieldName, item);

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

        public virtual bool EvaluateRulesFromField<T>(string fieldName, Item item) where T : RuleContext, new()
        {
            return EvaluateRulesFromField(new T(), fieldName, item);
        }

        public virtual bool ExecuteRulesFromField<T>(T ruleContext, string fieldName, Item item) where T : RuleContext
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            try
            {
                IEnumerable<Rule<RuleContext>> rules = RulesRepository.GetRules<RuleContext>(fieldName, item);

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

        public virtual bool ExecuteRulesFromField<T>(string fieldName, Item item) where T : RuleContext, new()
        {
            return ExecuteRulesFromField(new T(), fieldName, item);
        }
    }
}
