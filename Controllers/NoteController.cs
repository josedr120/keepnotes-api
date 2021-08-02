using System.Collections.Generic;
using System.Threading.Tasks;
using keepnotes_api.DTOs;
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
        public async Task<ActionResult<Note>> Get([FromRoute] string userId)
        {
            var userNotes = await _noteService.Get(userId);

            return Ok(userNotes);
        }

        [HttpGet("{noteId:length(24)}")]
        public async Task<ActionResult<Note>> GetById([FromRoute] string userId, string noteId)
        {
            var note = await _noteService.GetById(noteId, userId);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        
        [HttpPost("create")]
        public async Task<IActionResult> Create(Note note, [FromRoute] string userId)
        {
            if (string.IsNullOrEmpty(note.Title) || string.IsNullOrEmpty(note.Content))
            {
                return NoContent();
            }
            
            await _noteService.Create(note, userId);

            return Ok();
        }

        [HttpPut("{noteId:length(24)}")]
        public async Task<IActionResult> Update(string userId, string noteId, Note updatedNote)
        {
            if (noteId == null)
            {
                return NotFound();
            }
            await _noteService.Update(noteId, updatedNote);
            return NoContent();
        }

        [HttpDelete("{noteId:length(24)}")]
        public async Task<IActionResult> Delete(string noteId, string userId)
        {
            var note = await _noteService.GetById(noteId, userId);

            if (note == null)
            {
                return NotFound();
            }
            
            await _noteService.Delete(noteId);

            return NoContent();
        }
    }
}