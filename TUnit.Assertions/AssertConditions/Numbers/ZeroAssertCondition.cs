﻿using System.Numerics;

namespace TUnit.Assertions.AssertConditions.Numbers;

public class ZeroAssertCondition<TActual> : AssertCondition<TActual, TActual> 
    where TActual : INumber<TActual>, IEqualityOperators<TActual, TActual, bool>
{
    public ZeroAssertCondition(IReadOnlyCollection<AssertCondition<TActual, TActual>> nestedAssertConditions, NestedConditionsOperator? nestedConditionsOperator, TActual? expected) : base(nestedAssertConditions, nestedConditionsOperator, expected)
    {
    }

    public override string DefaultMessage => $"{ActualValue} is not equal to 0";

    protected override bool Passes(TActual actualValue)
    {
        return actualValue == TActual.Zero;
    }
}