using Firebase.Database;
using Firebase.Database.Query;
using GorselProgramlama_Yeni.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GorselProgramlama_Yeni.Services
{
    public class FirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService()
        {
            _firebaseClient = new FirebaseClient("https://gorselprogramlama-d9e4e-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        // T�m g�revleri getir (kullan�c�ya �zel filtreleme yok)
        public async Task<List<ToDoItem>> GetToDoItemsAsync()
        {
            return (await _firebaseClient
                .Child("todos")
                .OnceAsync<ToDoItem>())
                .Select(item =>
                {
                    var todo = item.Object;
                    todo.Id = item.Key;
                    return todo;
                }).ToList();
        }

        // G�rev ekle
        public async Task<string> AddToDoAsync(ToDoItem item)
        {
            var result = await _firebaseClient
                .Child("todos")
                .PostAsync(item);
            return result.Key;
        }

        // G�rev sil
        public async Task DeleteToDoAsync(string id)
        {
            await _firebaseClient
                .Child("todos")
                .Child(id)
                .DeleteAsync();
        }

        // G�rev g�ncelle
        public async Task UpdateToDoAsync(ToDoItem item)
        {
            await _firebaseClient
                .Child("todos")
                .Child(item.Id)
                .PutAsync(item);
        }
    }
}
