using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using keepnotes_api.DTOs;
using keepnotes_api.Helpers;
using keepnotes_api.Interfaces;
using keepnotes_api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace keepnotes_api.Services
{
    public class NoteService : INoteService
    {
        private readonly IMongoCollection<Note> _note;

        public NoteService(IKeepNotesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _note = database.GetCollection<Note>(settings.NotesCollectionName);
        }

        public async Task<List<Note>> Get(string userId)
        {
            
            var e = await _note.Find(notes => notes.UserId == userId).ToListAsync();
            
            // loops through all note objects and decrypts {title} and {content}
            e.ForEach(x =>
            {
                var decryptTitle = new Crypto().Decrypt(x.Title);
                var decryptContent = new Crypto().Decrypt(x.Content);
                
                x.Title = decryptTitle;
                x.Content = decryptContent;

                /*userNotes.Id = x.Id;
                userNotes.UserId = x.UserId;
                userNotes.Title = x.Title;
                userNotes.Content = x.Content;*/
                
                
            });


            return e;
        }

        public async Task<NoteDto> GetById(string noteId, string userId = "")
        {
            var e = await _note.Find(note => note.UserId == userId && note.Id == noteId).FirstOrDefaultAsync();
            var decryptTitle = new Crypto().Decrypt(e.Title);
            var decryptContent = new Crypto().Decrypt(e.Content);
            
            e.Title = decryptTitle;
            e.Content = decryptContent;

            var userNote = new NoteDto()
            {
                Id = e.Id,
                UserId = e.UserId,
                Title = e.Title,
                Content = e.Content
            };


            return userNote;
        }

        public async Task<Note> Create(Note note, string userId)
        {
            var titleEncrypt = new Crypto().Encrypt(note.Title);
            var contentEncrypt = new Crypto().Encrypt(note.Content);
            
            note.UserId = userId;
            note.Title = titleEncrypt;
            note.Content = contentEncrypt;
            await _note.InsertOneAsync(note);

            return note;
        }

        public async Task<bool> Update(string noteId, Note note)
        {
            var titleEncrypt = new Crypto().Encrypt(note.Title);
            var contentEncrypt = new Crypto().Encrypt(note.Content);
            var filter = Builders<Note>.Filter.Eq(x => x.Id, noteId);
            var update = Builders<Note>.Update
                .Set(x => x.Title, titleEncrypt)
                .Set(x => x.Content, contentEncrypt);

            var result = await _note.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string noteId)
        {
            var filter = Builders<Note>.Filter.Eq(x => x.Id, noteId);
            var result = await _note.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }
    }
}