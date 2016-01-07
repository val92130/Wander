namespace Wander.Server.ClassLibrary.Model.Players
{
    /// <summary>
    /// Represent a Wander Game Player from the Server Side, these objects should NOT be transfered to the clients
    /// </summary>
    public class ServerPlayerModel
    {
        public int UserId { get; set; }
        public string SignalRId { get; set; }
        public string Pseudo { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 SavedPosition { get; set; }
        public EPlayerDirection Direction { get; set; }
        public int MapId { get; set; }

        public ServerPlayerModel()
        {
            this.MapId = -1;
        }
    }
}
