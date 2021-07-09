using System.Collections.Generic;
using keepnotes_api.Models;
using keepnotes_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace keepnotes_api.Controllers
{
    [Authorize]
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NoteService _noteService;

        public NoteController(NoteService noteService)
        {
            _noteService = noteService;
        }
        
        [HttpGet("{userId:length(24)}")]
        public ActionResult<List<Note>> GetAllNotes([FromRoute] string userId)
        {
            var userNotes = _noteService.GetAllNotes(userId);

            return Ok(userNotes);
        }

        [HttpGet("{userId:length(24)}/{id:length(24)}")]
        public ActionResult<Note> GetNoteById([FromRoute] string userId, string id)
        {
            var note = _noteService.GetNoteById(id, userId);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        
        [HttpPost("{userId:length(24)}/create")]
        public ActionResult<Note> CreateNote(Note note, [FromRoute] string userId)
        {
            _noteService.CreateNote(note, userId);

            return Ok(note);
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult<Note> UpdateNote(string id, Note updatedNote)
        {
            var note = _noteService.GetNoteById(id);

            if (note == null)
            {
                return NotFound();
            }
            
            _noteService.UpdateNote(id, updatedNote);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult<Note> DeleteNoteById(string id)
        {
            var note = _noteService.GetNoteById(id);

            if (note == null)
            {
                return NotFound();
            }
            
            _noteService.DeleteNote(id);

            return NoContent();
        }
    }
}