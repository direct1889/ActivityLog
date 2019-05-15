/*
* AUTHORITY : tempura
* URL : https://qiita.com/tempura/items/4a5482ff6247ec8873df
* LICENSE : UNK
*/

using UnityEngine;
using UTouch = UnityEngine.Touch;
using System;
using static du.di.ExTouchStep;
using static du.Ex.ExVector;

namespace du.di {

	/// <summary>
	/// タッチ段階。UnityEngine.TouchPhase に None の情報を追加拡張。
	/// </summary>
	[Flags]
	public enum TouchStep {
		// -----------------------------------
		/// <summary>
		/// タッチ中(ビットフラグ用)
		/// 末尾の桁が1か否かで区別
		/// </summary>
		Touching = 0b0001,
		// -----------------------------------

		None = 0, // タッチなし

		// 以下は UnityEngine.TouchPhase の値に対応
		Began = Touching, // タッチ開始
		/// <summary>
		/// タッチ中かつ移動中
		/// </summary>
		Moved = Touching | 0b0010,
		/// <summary>
		/// タッチ中かつ静止中
		/// </summary>
		Stationary = Touching | 0b0100,

		Ended = 0b0010, // タッチ終了
		/// <summary>
		/// システムがタッチの追跡をキャンセルしたとき
		/// </summary>
		Canceled = 0b0100
	}

	public static class ExTouchStep {

		public static TouchStep Phase2Step(this TouchPhase phase) {
			switch (phase) {
				case TouchPhase.Began		: return TouchStep.Began;
				case TouchPhase.Moved		: return TouchStep.Moved;
				case TouchPhase.Stationary	: return TouchStep.Stationary;
				case TouchPhase.Ended		: return TouchStep.Ended;
				case TouchPhase.Canceled	: return TouchStep.Canceled;
				default						: return TouchStep.None;
			}
		}

		public static bool IsTouching(this TouchStep own) {
			return (own & TouchStep.Touching) != 0;
		}

	}

}
