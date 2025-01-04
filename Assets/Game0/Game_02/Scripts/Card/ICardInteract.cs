namespace Card
{
    public interface ICardInteract
    {
        public bool IsInteractable();
        public void UseCard();

        public void OnClickObject();
    }
}
