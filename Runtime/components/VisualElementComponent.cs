using Unity.Entities;
using UnityEngine.UIElements;

public partial class VisualElementComponent : IComponentData, IValueComponent<VisualElement>
{
    public VisualElement Value;

    VisualElement IValueComponent<VisualElement>.ValueOuter
    {
        get => Value;
        set => Value = value;
    }
}
