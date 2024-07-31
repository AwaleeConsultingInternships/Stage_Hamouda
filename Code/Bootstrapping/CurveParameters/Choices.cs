namespace Bootstrapping.CurveParameters
{
    public enum InterpolationChoice
    {
        UsingDirectSolving,
        UsingNewtonSolver,
        UsingFuturesMain
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

    public enum ConvexityChoice
    {
        NoConvexity,
        HullApproachWithVasicek,
        HullApproachWithHoLee,
        MartingaleApproachWithVasicek,
        MartingaleApproachWithHoLee
    }
}
