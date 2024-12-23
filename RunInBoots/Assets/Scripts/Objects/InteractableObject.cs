using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public abstract class InteractableObject : MonoBehaviour
{
    public int targetStage;
    public int targetIndex;
    public UnityEvent Interact = new UnityEvent();

    //public void Initialize()
    //{

    //}

    //public void Interact()
    //{
    //    Interact.Invoke();
    //}

    protected virtual void Start()
    {
        Interact.AddListener(OnInteract);
    }

    protected abstract void OnInteract();

    protected void LoadScene(int stage, int index)
    {
        SceneManager.LoadScene($"Stage_{stage}_{index}");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interact.Invoke();
            Debug.Log("Player interacted with " + gameObject.name);
        }
    }
}
