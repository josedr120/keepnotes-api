namespace keepnotes_api.Interfaces
{
    public interface IKeepNotesDatabaseSettings
    {
         string UsersCollectionName { get; set; }
         string NotesCollectionName { get; set; }
         string ConnectionString { get; set; }
         string DatabaseName { get; set; }
    }
}