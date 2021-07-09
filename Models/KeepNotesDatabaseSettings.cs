using keepnotes_api.Interfaces;

namespace keepnotes_api.Models
{
    public class KeepNotesDatabaseSettings: IKeepNotesDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        
        public string NotesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        
    }
    
   
}