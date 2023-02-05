using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupBase : MonoBehaviour {
  public enum PowerupType { Health, Shield, Weapon }
  protected PowerupType powerupType;

}