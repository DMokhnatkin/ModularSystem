namespace ModularSystem.Common.Dependencies
{
    public interface ICheckResult
    {
        bool IsSuccess { get; }

        string GetMessage();
    }
}
