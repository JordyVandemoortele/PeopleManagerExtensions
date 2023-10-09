namespace Vives.Services.Model.Extensions
{
    public static class ServiceResultExtensions
    {
        public static ServiceResult<T> NotFound<T>(this ServiceResult<T> result, string entity)
        {
            result.Messages.Add(new ServiceMessage
            {
                Code = "NotFound",
                Type = ServiceMessageType.Error,
                Title = $"Could not find {entity}.",
                Description = $"We were unable to find the requested {entity}."
            });
            return result;
        }
    }
}
