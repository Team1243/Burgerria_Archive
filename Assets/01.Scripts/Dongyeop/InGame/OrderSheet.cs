using UnityEngine.UIElements;

public class OrderSheet 
{
    public VisualElement BackImage;
    public VisualElement Burger;

    public OrderSheet(VisualElement backImage)
    {
        BackImage = backImage;
        Burger = backImage.Q<VisualElement>("burger");
    }
}
