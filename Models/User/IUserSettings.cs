namespace keepnotes_api.Models.User
{
    public interface IUserSettings
    {
        string Theme { get; set; }
        
        string Language { get; set; }
    }
}