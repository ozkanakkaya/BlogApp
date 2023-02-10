namespace BlogApp.Core.Enums
{
    public enum ResultStatus
    {
        Success = 200,
        Created = 201,
        NoContent = 204,
        Error = 1,
        Warning = 2, // ResultStatus.Warning
        Info = 3, // ResultStatus.Info,
        Authentication = 4,
        Authorization = 5,
    }
}
