using UnityEngine;

public class PanelBase : MonoBehaviour
{
    public virtual void Show()
    {
        this.gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
