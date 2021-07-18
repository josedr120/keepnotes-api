using System.Collections.Generic;
using System.Security.Cryptography;
using keepnotes_api.Helpers;
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
            
            // loops through all note objects and decrypts {title} and {content}
            userNotes.ForEach(x =>
            {
                var decryptTitle = new Crypto().Decrypt(x.Title);
                var decryptContent = new Crypto().Decrypt(x.Content);
                
                x.Title = decryptTitle;
                x.Content = decryptContent;
            });


            return userNotes;
        }

        public Note GetNoteById(string id, string userId = "")
        {
            var userNote = _note.Find(note => note.UserId == userId && note.Id == id).FirstOrDefault();
            var decryptTitle = new Crypto().Decrypt(userNote.Title);
            var decryptContent = new Crypto().Decrypt(userNote.Content);
            
            userNote.Title = decryptTitle;
            userNote.Content = decryptContent;

            return userNote;
        }

        public Note CreateNote(Note note, string userId)
        {
            var titleEncrypt = new Crypto().Encrypt(note.Title);
            var contentEncrypt = new Crypto().Encrypt(note.Content);
            
            note.UserId = userId;
            note.Title = titleEncrypt;
            note.Content = contentEncrypt;
            _note.InsertOne(note);

            return note;
        }

        public void UpdateNote(string userId, string noteId, Note updatedNote)
        {


            var titleEncrypt = new Crypto().Encrypt(updatedNote.Title);
            var contentEncrypt = new Crypto().Encrypt(updatedNote.Content);

            updatedNote.UserId = userId;
            updatedNote.Title = titleEncrypt;
            updatedNote.Content = contentEncrypt;


            _note.ReplaceOne(note => note.Id == noteId, updatedNote);
        }

        public void DeleteNote(string id) => _note.DeleteOne(note => note.Id == id);
    }
}