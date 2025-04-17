namespace ShopClient
{
    public class ModalManager
    {
        public event Action<string> OnShow;

        public void Show(string modalName)
        {
            OnShow?.Invoke(modalName);
        }
    }
}
