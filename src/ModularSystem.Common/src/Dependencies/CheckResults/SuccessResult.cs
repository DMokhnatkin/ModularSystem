namespace ModularSystem.Common.Dependencies
{
    public class SuccessResult : ICheckResult
    {
        /// <inheritdoc />
        public bool IsSuccess => true;

        /// <inheritdoc />
        public string GetMessage()
        {
            return "Resolve was success.";
        }
    }
}
