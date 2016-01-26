namespace Wander.Server.ClassLibrary.Services.Interfaces
{
    public interface IMapService
    {
        byte[] GetMap();
        bool IsCollision(int x, int y);
        int GetWidth();
        int GetHeight();
        int GetTileWidth();
        int GetTileHeight();
    }
}