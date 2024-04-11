using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;
using Animation;
using UnityEngine.Audio;

namespace Enemy
{

	public class EnemyBase : MonoBehaviour, IDamageable

	{
		public Collider _collider;
		public FlashColor flashColor;
		public ParticleSystem _particleSystem;
		public AudioSource audiosourceKill;


		public float startLife = 10f;
		public bool lookAtPlayer = false;


		[SerializeField] private float _currentLife;

		[Header("Animation")]
		[SerializeField] private AnimationBase animationBase;



		[Header("Start Animation")]
		public float startAnimationDuration = .2f;
		public Ease startAnimationEase = Ease.OutBack;
		public bool startWithBornAnimation = true;



		[Header("Event")]
		public UnityEvent OnKillEvent;

		private Player _player;


		private void Awake()
		{
			Init();

		}


		private void Start()
		{
			_player = GameObject.FindObjectOfType<Player>();
		}


		protected void ResetLife()
		{
			_currentLife = startLife;
		}


		protected virtual void Init()
		{
			ResetLife();
			if (startWithBornAnimation)
				BornAnimation();
		}


		protected virtual void Kill()
		{
			OnKill();
		}


		protected virtual void OnKill()
		{
			if (_collider != null) _collider.enabled = false;
			Destroy(gameObject, 3f);
			PlayAnimationByTrigger(AnimationType.DEATH);
			if (audiosourceKill != null) audiosourceKill.Play();
			OnKillEvent?.Invoke();
		}



		public void OnDamage(float f)
		{
			if (flashColor != null) flashColor.Flash();
			if (_particleSystem != null) _particleSystem.Emit(15);

			transform.position -= transform.forward;

			_currentLife -= f;

			if (_currentLife <= 0)
			{
				Kill();
			}
		}



		#region ANIMATION


		private void BornAnimation()
		{
			transform.DOScale(0, startAnimationDuration).SetEase(startAnimationEase).From();
		}

		public void PlayAnimationByTrigger(AnimationType animationType)
		{
			animationBase.PlayAnimationByTrigger(animationType);
		}

		#endregion




		//debug


		public void Damage(float damage)
		{
			//Debug.Log("Damage");
			OnDamage(damage);
		}

		public void Damage(float damage, Vector3 dir)
		{
			OnDamage(damage);
			transform.DOMove(transform.position - dir, .1f);

		}

		private void OnCollisionEnter(Collision collision)
		{
			Player p = collision.transform.GetComponent<Player>();
			if (p != null)
			{
				p.healthBase.Damage(1);
			}

		}

		public virtual void Update()
		{
			if (lookAtPlayer)
			{
				transform.LookAt(_player.transform.position);
			}
		}



	}

}


