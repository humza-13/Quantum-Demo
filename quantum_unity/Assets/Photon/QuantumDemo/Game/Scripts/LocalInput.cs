using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;
using Input = UnityEngine.Input;

public class LocalInput : MonoBehaviour {
    
  private void OnEnable() {
    QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
  }

  public void PollInput(CallbackPollInput callback) {
    Quantum.Input i = new Quantum.Input();
    i.Jump = Input.GetButton("Jump");
    i.Direction = new Vector2(Input.GetAxis("Horizontal"),
      Input.GetAxis("Vertical")).ToFPVector2();
    callback.SetInput(i, DeterministicInputFlags.Repeatable);
  }
}
