using System.Collections.Generic;
using keepnotes_api.Models;
using keepnotes_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace keepnotes_api.Controllers
{
    /*[Authorize]*/
    [Route("api/note/{userId:length(24)}")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly NoteService _noteService;

        public NoteController(NoteService noteService)
        {
            _noteService = noteService;
        }
        
        [HttpGet("")]
        public ActionResult<List<Note>> GetAllNotes([FromRoute] string userId)
        {
            var userNotes = _noteService.GetAllNotes(userId);

            return Ok(userNotes);
        }

        [HttpGet("{noteId:length(24)}")]
        public ActionResult<Note> GetNoteById([FromRoute] string userId, string noteId)
        {
            var note = _noteService.GetNoteById(noteId, userId);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        
        [HttpPost("create")]
        public ActionResult<Note> CreateNote(Note note, [FromRoute] string userId)
        {
            _noteService.CreateNote(note, userId);

            return Ok(note);
        }

        [HttpPut("{noteId:length(24)}")]
        public ActionResult<Note> UpdateNote([FromRoute] string userId, string noteId, Note updatedNote)
        {
            

            if (noteId == null)
            {
                return NotFound();
            }
            
            _noteService.UpdateNote(userId, noteId, updatedNote);
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