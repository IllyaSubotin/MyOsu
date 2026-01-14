using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthManager
{
    Action OnHealthEnded { get; set; }
    void HealthReset();
    void UpdateHealth(HitResult hitResult);
}
