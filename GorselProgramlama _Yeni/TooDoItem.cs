namespace GorselProgramlama_Yeni.Models
{
    public class ToDoItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Detail { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public bool IsDone { get; set; } = false;

    }
}
