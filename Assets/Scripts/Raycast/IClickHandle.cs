namespace Raycast
{
    public interface IClickHandle
    {
        public void OnClickObject();
        
        public void OnDragObject();
        
        public void EndObject();
    }
}
