using System.Collections.Generic;
using System.Security.Cryptography;
using keepnotes_api.Interfaces;
using keepnotes_api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace keepnotes_api.Services
{
    public class NoteService
    {
        private readonly IMongoCollection<Note> _note;
        
        public NoteService(IKeepNotesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _note = database.GetCollection<Note>(settings.NotesCollectionName);
        }

        public List<Note> GetAllNotes(string userId)
        {
            var userNotes = _note.Find(notes => notes.UserId == userId).ToList();

            return userNotes;
        }

        public Note GetNoteById(string id, string userId = "")
        {
            var userNote =_note.Find(note => note.UserId == userId && note.Id == id).FirstOrDefault();

            return userNote;
        }

        public Note CreateNote(Note note, string userId)
        {
            note.UserId = userId;
            _note.InsertOne(note);

            return note;
        }

        public void UpdateNote(string id, Note updatedNote) => _note.ReplaceOne(note => note.Id == id, updatedNote);

        public void DeleteNote(string id) => _note.DeleteOne(note => note.Id == id);
    }
}