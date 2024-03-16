using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUp
{
    public abstract void Deploy();
    public abstract void Handle();
    public abstract void  Destroy();
}
