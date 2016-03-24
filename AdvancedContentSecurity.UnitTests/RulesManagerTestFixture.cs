using System;
using System.Collections.Generic;
using AdvancedContentSecurity.Core.Rules;
using ContentSecurity.Core.Rules;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data.Items;
using Sitecore.Rules;

namespace AdvancedContentSecurity.UnitTests
{
    [TestFixture]
    public class RulesManagerTestFixture
    {
        #region [ Evaluate Tests ]

        [Test]
        public void EvaluateRulesFromField_for_correct_data_returns_true()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            testHarness.RulesRepository.Evaluate(Arg.Any<RuleContext>(), Arg.Any<IEnumerable<Rule<RuleContext>>>()).Returns(true);

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item rulesItem = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Item itemToEvaluate = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            // Act
            bool result = testHarness.RulesManager.EvaluateRulesFromField<RuleContext>("fieldName", rulesItem, itemToEvaluate);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void EvaluateRulesFromField_with_supplied_rule_context_for_correct_data_returns_true()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            RuleContext ruleContext = new RuleContext();
            testHarness.RulesRepository.Evaluate(ruleContext, Arg.Any<IEnumerable<Rule<RuleContext>>>()).Returns(true);

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item rulesItem = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            Item itemToEvaluate = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            ruleContext.Item = itemToEvaluate;

            // Act
            bool result = testHarness.RulesManager.EvaluateRulesFromField(ruleContext, "fieldName", rulesItem);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void EvaluateRulesFromField_with_empty_rules_returns_true()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            RuleContext ruleContext = new RuleContext();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            testHarness.RulesRepository.GetRules<RuleContext>("fieldName", item).Returns(null as IEnumerable<Rule<RuleContext>>);

            // Act
            bool result = testHarness.RulesManager.EvaluateRulesFromField(ruleContext, "fieldName", item);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void EvaluateRulesFromField_for_correct_data_returns_false()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            testHarness.RulesRepository.Evaluate(Arg.Any<RuleContext>(), Arg.Any<IEnumerable<Rule<RuleContext>>>()).Returns(false);

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item rulesItem = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            Item itemToEvaluate = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            // Act
            bool result = testHarness.RulesManager.EvaluateRulesFromField<RuleContext>("fieldName", rulesItem, itemToEvaluate);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void EvaluateRulesFromField_with_supplied_rule_context_for_correct_data_returns_false()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            RuleContext ruleContext = new RuleContext();
            testHarness.RulesRepository.Evaluate(ruleContext, Arg.Any<IEnumerable<Rule<RuleContext>>>()).Returns(false);

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item rulesItem = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            Item itemToEvaluate = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            ruleContext.Item = itemToEvaluate;

            // Act
            bool result = testHarness.RulesManager.EvaluateRulesFromField(ruleContext, "fieldName", rulesItem);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void EvaluateRulesFromField_repository_exception_returns_false()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            RuleContext ruleContext = new RuleContext();
            testHarness.RulesRepository.When(x => x.Evaluate(ruleContext, Arg.Any<IEnumerable<Rule<RuleContext>>>())).Throw<Exception>();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            // Act
            bool result = testHarness.RulesManager.EvaluateRulesFromField(ruleContext, "fieldName", item);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void EvaluateRulesFromField_with_null_item_excepts()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            RuleContext ruleContext = new RuleContext();
            testHarness.RulesRepository.Evaluate(ruleContext, Arg.Any<IEnumerable<Rule<RuleContext>>>()).Returns(false);

            // Act
            Assert.Throws<ArgumentNullException>(() => testHarness.RulesManager.EvaluateRulesFromField(ruleContext, "fieldName", null));

            // Assert
        }

        #endregion

        #region [ Execution Tests ]

        [Test]
        public void ExecuteRulesFromField_for_correct_data_returns_true()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            Item itemToEvaluate = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            // Act
            bool result = testHarness.RulesManager.ExecuteRulesFromField<RuleContext>("fieldName", item, itemToEvaluate);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void ExecuteRulesFromField_with_supplied_rule_context_for_correct_data_returns_true()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            RuleContext ruleContext = new RuleContext();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            // Act
            bool result = testHarness.RulesManager.ExecuteRulesFromField(ruleContext, "fieldName", item);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void ExecuteRulesFromField_with_empty_rules_returns_true()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            RuleContext ruleContext = new RuleContext();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            testHarness.RulesRepository.GetRules<RuleContext>("fieldName", item).Returns(null as IEnumerable<Rule<RuleContext>>);

            // Act
            bool result = testHarness.RulesManager.ExecuteRulesFromField(ruleContext, "fieldName", item);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void ExecuteRulesFromField_for_correct_data_returns_false()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            testHarness.RulesRepository.When(x => x.Execute(Arg.Any<RuleContext>(), Arg.Any<IEnumerable<Rule<RuleContext>>>())).Throw<Exception>();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item rulesItem = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);
            Item itemToEvaluate = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            // Act
            bool result = testHarness.RulesManager.ExecuteRulesFromField<RuleContext>("fieldName", rulesItem, itemToEvaluate);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void ExecuteRulesFromField_with_supplied_rule_context_for_correct_data_returns_false()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            testHarness.RulesRepository.When(x => x.Execute(Arg.Any<RuleContext>(), Arg.Any<IEnumerable<Rule<RuleContext>>>())).Throw<Exception>();
            RuleContext ruleContext = new RuleContext();

            Guid itemId = Guid.NewGuid();
            Guid templateId = Guid.NewGuid();
            Guid branchId = Guid.Empty;
            string itemName = "fred";

            Item item = TestUtilities.GetTestItem(itemId, templateId, branchId, itemName);

            // Act
            bool result = testHarness.RulesManager.ExecuteRulesFromField(ruleContext, "fieldName", item);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void ExecuteRulesFromField_with_null_item_excepts()
        {
            // Arrange
            RulesManagerTestHarness testHarness = new RulesManagerTestHarness();
            RuleContext ruleContext = new RuleContext();

            // Act
            Assert.Throws<ArgumentNullException>(() => testHarness.RulesManager.ExecuteRulesFromField(ruleContext, "fieldName", null));

            // Assert
        }

        #endregion

        public class RulesManagerTestHarness
        {
            public RulesManagerTestHarness()
            {
                RulesRepository = Substitute.For<IRulesRepository>();
                RulesManager = new RulesManager(RulesRepository);
            }

            public IRulesManager RulesManager { get; private set; }

            public IRulesRepository RulesRepository { get; private set; }
        }
    }
}
