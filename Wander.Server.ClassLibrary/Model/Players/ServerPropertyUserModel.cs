namespace Wander.Server.ClassLibrary.Model.Players
{
    public class ServerPropertyUserModel
    {
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyDescription { get; set; }
        public int Threshold { get; set; }
        public int Price { get; set; }
        public int UserId { get; set; }
    }
}