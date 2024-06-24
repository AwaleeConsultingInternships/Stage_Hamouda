namespace Bootstrapping.CurveParameters
{
    public enum InterpolationChoice
    {
        UsingDirectSolving,
        UsingNewtonSolver,
        UsingRawData
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
