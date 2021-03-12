using UnityEngine;

public class CachedComponent<TComponent>
{
    private TComponent component;

    public TComponent Get(MonoBehaviour parent)
    {
        if(component == null)
        {
            component = parent.GetComponent<TComponent>();
        }
        return component;
    }
}
