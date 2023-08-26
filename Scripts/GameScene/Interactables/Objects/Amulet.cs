namespace GameScene.Interactables.Objects
{
    public class Amulet : InteractableObject
    {
        private void Start()
        {
            base.Initialize();
        }

        protected override void Interact()
        {
            base.Interact();
            Destroy(gameObject);
        }
    }
}