using UnityEngine;
using System.Collections;

public class Locomotion2D {

	private Animator _animator = null;
	private int _speedId = 0;
	private int _jumpId = 0;

	// Use this for initialization
	public Locomotion2D(Animator animator) {
		_animator = animator;
		_speedId = Animator.StringToHash("Speed");
		_jumpId = Animator.StringToHash("Jump");
	}

	public void Update (bool jump, float speed) {
		_animator.SetFloat (_speedId, speed);
		AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
		if (jump && !state.IsName("Jump")) {
			_animator.SetBool (_jumpId, true);
		}
		else if (state.IsName("Jump")) {
			_animator.SetBool (_jumpId, false);
		}
	}
}
