using System.Collections.Generic;
using System.Threading.Tasks;
using keepnotes_api.DTOs;
using keepnotes_api.Models;

namespace keepnotes_api.Services
{
    public interface INoteService
    {
        
        Task<NoteDto> GetById(string noteId, string userId = "");
        
        Task<List<Note>> Get(string userId);

        Task<Note> Create(Note note, string userId);

        Task<bool> Update(string noteId, Note note);

        Task<bool> Delete(string noteId);
    }
}