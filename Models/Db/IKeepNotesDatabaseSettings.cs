namespace keepnotes_api.Models.Db
{
    public interface IKeepNotesDatabaseSettings
    {
         string UsersCollectionName { get; set; }
         string NotesCollectionName { get; set; }
         string ConnectionString { get; set; }
         string DatabaseName { get; set; }
    }
}