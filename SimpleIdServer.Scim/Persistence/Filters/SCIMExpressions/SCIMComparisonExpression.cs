﻿namespace SimpleIdServer.Persistence.Filters.SCIMExpressions
{
    public class SCIMComparisonExpression : SCIMExpression
    {
        public SCIMComparisonExpression(SCIMComparisonOperators comparisonOperator, SCIMAttributeExpression leftExpression, string value)
        {
            ComparisonOperator = comparisonOperator;
            LeftExpression = leftExpression;
            Value = value;
        }

        public SCIMComparisonOperators ComparisonOperator { get; private set; }
        public SCIMAttributeExpression LeftExpression { get; private set; }
        public string Value { get; private set; }

        public override object Clone()
        {
            return new SCIMComparisonExpression(ComparisonOperator, (SCIMAttributeExpression)LeftExpression.Clone(), Value);
        }
    }
}
