using UnityEngine.UIElements;

public abstract partial class VisualElementUI : UIElementSystem<VisualElement, VisualElementComponent>
{
    public VisualElement VisualElement => Element;
}
