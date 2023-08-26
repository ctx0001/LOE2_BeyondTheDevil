namespace GameScene.Data.Handlers.Dependencies
{
    public class Assignment
    {
        public int id { get; set; }
        public string description { get; set; }
        public string hint { get; set; }
        public bool active { get; set; }
        public bool completed { get; set; }

        public Assignment(int id, string description, string hint, bool active, bool completed)
        {
            this.id = id;
            this.description = description;
            this.hint = hint;
            this.active = active;
            this.completed = completed;
        }
        
    }
}