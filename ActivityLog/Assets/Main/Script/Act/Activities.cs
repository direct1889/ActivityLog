using UnityEngine;

namespace Main.Act {

	/// <summary> アクティビティの系列(日毎) </summary>
	public interface IROActivitiesContainer {

		/// <summary> 時系列順で index 番目 </summary>
		/// <param name="index"> index 番目が存在しない場合例外を投げる </param>
		// IActivity this [int index] { get; }

		/// <summary> 指定時刻に行われていたアクティビティ </summary>
		IROActivity this [MinuteOfDay time] { get; }

		/// <summary> 指定時刻に行われていたアクティビティの index </summary>
		int IndexOf(MinuteOfDay time);

		/// <summary> 末尾のアクティビティ </summary>
		IROActivity Back { get; }

	}
	public interface IActivitiesContainer : IROActivitiesContainer {

		/// <summary> 末尾に追加 </summary>
		void PushBack(IActivity act);

		/// <summary>
		/// 指定した時間に挿入
		/// 完全に包含されるアクティビティは消滅
		/// </summary>
		void Insert(MinuteOfDay begin, MinuteOfDay end, IROContent cnt);

		/// <summary> 直前のアクティビティが伸びる </summary>
		void RemoveAt(int index);

		/// <summary> 登録済みのアクティビティを移動させる </summary>
		/// <param name="index"> 変更対象の index </param>
		void Move(int index, MinuteOfDay newBegin, MinuteOfDay newEnd);

		/// <summary> 登録済みのアクティビティの内容を書き換える </summary>
		/// <param name="index"> 変更対象の index </param>
		void Overwrite(int index, IROContent newContent);

	}

	/// <summary> アクティビティ系列/日を操作する </summary>
	public interface IActivitiesMgr {
		/// <summary>
		/// 新たなアクティビティを開始
		/// 現在のアクティビティを終了、開始時刻は現在時刻を使用
		/// </summary>
		void BeginNewAct(IProject proj, string name, bool isEffective);
		void BeginNewAct(IProject proj, string name);
		/// <summary>
		/// 連続したアクティビティ間の時刻境界を操作
		/// </summary>
		/// <param name="newMinute">
		/// ∈ (justBefore.Begin, justAfter.End)
		/// 開区間外だと(=隣接アクティビティが潰れると)失敗
		/// </param>
		void ChangeBorder(int lastIndexBeforeBorder, MinuteOfDay newMinute);
	}


}
