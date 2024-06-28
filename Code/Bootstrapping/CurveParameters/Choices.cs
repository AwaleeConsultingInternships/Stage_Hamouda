﻿namespace Bootstrapping.CurveParameters
{
    public enum InterpolationChoice
    {
        UsingDirectSolving,
        UsingNewtonSolver
    }

    public enum InterpolationMethod
    {
        LinearOnYield,
        LinearOnYieldLog
    }

    public enum DataChoice
    {
        RawData,
        InterpolatedData
    }

    public enum VariableChoice
    {
        Discount,
        Yield
    }
}
