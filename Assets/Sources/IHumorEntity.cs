using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHumorEntity {
   
    void GainHumor(int humor, int quantity);

    void LoseHumor(int humor, int quantity);
}
