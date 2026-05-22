using UnityEngine;

public interface ICounterable
{
    public bool CanBeCountered { get; set;}
    public void HandleCounter();
}
