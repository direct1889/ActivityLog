using System.Collections.Generic;
using UnityEngine;

namespace Main.Act {

    /// <summary> アクティビティの系列(日毎) </summary>
    public class ActivitiesContainer : IActivitiesContainer {
        #region field
        IList<IActivity> m_acts;
        #endregion

        #region ctor/dtor
        public ActivitiesContainer() { m_acts = new List<IActivity>(); }
        #endregion

        // IROActivitiesContainer -------
        #region getter
        public IActivity this [int index] { get { return m_acts[index]; } }
        public IROActivity this [MinuteOfDay time] { get { return m_acts[IndexOf(time)]; } }
        public int IndexOf(MinuteOfDay time) {
            if (MinuteOfDay.Now < time) {
                throw new System.ArgumentException("Argument is future.");
            }
            else {
                for (int i = m_acts.Count - 1; i >= 0; --i) {
                    if (m_acts[i].Context.BeginTime < time) {
                        return i;
                    }
                }
            }
            // ここには本来到達し得ない
            throw new System.Exception("");
        }

        public int Count { get { return m_acts.Count; } }
        public IROActivity Back { get { return m_acts[m_acts.Count - 1]; } }
        private IActivity MutableBack { get { return m_acts[m_acts.Count - 1]; } }
        private bool IndexIsValid(int index) { return 0 <= index && index < m_acts.Count; }
        #endregion
        // ------------------------------

        #region public
        public void PushBack(IActivity act) {
            if (m_acts.Count == 0) {
                m_acts.Add(act);
            }
            else if (Back.Context.BeginTime < act.Context.BeginTime) {
                Debug.LogError($"PushBack[{Back.Context.BeginTime} >< {act.Context.BeginTime}]");
                MutableBack.MutableContext.ResetFollowAct(act);
                m_acts.Add(act);
            }
            else {
                Debug.LogError($"PushBack[{Back.Context.BeginTime} >< {act.Context.BeginTime}]");
                for (int i = Count - 1; i > IndexOf(act.Context.BeginTime); --i) {
                    RemoveAt(i);
                }
                PushBack(act);
            }
        }
        public void Insert(MinuteOfDay begin, MinuteOfDay end, IROContent cnt) {
            IIndependentActivity act = new IndependentActivity(cnt, new IndependentContext(begin, end));
            int justBefore = IndexOf(act.IndependentContext.BeginTime);
            int justAfter = IndexOf(act.IndependentContext.EndTime);
            // e.g.        |---- act ----|
            // |-- justBefore --|- In -|-- justAfter --| の場合

            // (1)包含アクティビティを削除
            //             |---- act ----|
            // |-- justBefore ---------|-- justAfter --|
            for (int i = justAfter - 1; i > justBefore; --i) {
                RemoveAt(i); // 後ろから消さないとindexがずれる
            }
            justAfter = justBefore + 1;
            // (2)挿入位置の直後のアクティビティ -> Precede を書き換え
            //             |---- act ----|
            // |-- jB -------------------|----- jA ----|
            m_acts[justAfter].MutableContext.ResetPrecedeAct(act);
            // (3)MakeDependしてコンテナへ
            //             |---- act ----|----- jA ----|
            // |-- jB -------------------|----- jA ----|
            m_acts.Insert(justAfter, act.MakeDepend(m_acts[justAfter]));
            // (4)挿入位置の直前のアクティビティ -> Follow を書き換え
            // |-- jB -----|---- act ----|----- jA ----|
            m_acts[justBefore].MutableContext.ResetFollowAct(m_acts[justAfter]);
        }
        public void RemoveAt(int index) {
            // 無効な index : 削除自体しない
            if (!IndexIsValid(index)) { return; }
            // 先頭以外 : 直前のアクティビティを直後の開始時刻まで延長
            // > 末尾 : 直前のアクティビティを再開
            else if (index > 0) {
                m_acts[index - 1].MutableContext.ResetFollowAct(m_acts[index].MutableContext.NextAct);
            }
            // 先頭 : 直後のアクティビティの開始時刻を00:00に伸ばす
            else {
                m_acts[index + 1].MutableContext.ResetPrecedeAct(MinuteOfDay.Begin);
            }
            m_acts.RemoveAt(index);
        }
        public void Move(int index, MinuteOfDay newBegin, MinuteOfDay? newEnd) {
            var act = m_acts[index];
            RemoveAt(index);
            if (newEnd != null) { Insert(newBegin, (MinuteOfDay)newEnd, act.Content); }
            else { PushBack(act); }
        }
        public void OverwriteCnt(int index, IROContent newContent) {
            m_acts[index].ResetContent(newContent);
        }
        public void OverwriteBeginTime(int index, MinuteOfDay newBegin) {
            Move(index, newBegin, m_acts[index].Context.EndTime);
        }
        public void OverwriteEndTime(int index, MinuteOfDay? newEnd) {
            Move(index, m_acts[index].Context.BeginTime, newEnd);
        }
        #endregion
    }

    /// <summary> アクティビティ系列/日を操作する </summary>
    public class ActivitiesMgr : IActivitiesMgr {
        #region property
        public IROActivitiesContainer Activities { get { return Acts; } }
        protected IActivitiesContainer Acts { get; }
        #endregion

        #region ctor/dtor
        public ActivitiesMgr() { Acts = new ActivitiesContainer(); }
        #endregion

        #region public
        public void BeginNewAct(IProject proj, string name, bool isEffective) {
            Acts.PushBack(new Activity(new Content(proj, name, isEffective), Context.BeginFromNow));
        }
        public void BeginNewAct(IProject proj, string name) {
            Acts.PushBack(new Activity(new Content(proj, name), Context.BeginFromNow));
        }
        public void ChangeBorder(int indexJustAfterBorder, MinuteOfDay newMinute) {
            if (indexJustAfterBorder <= 0) { return; }
            else if (Acts[indexJustAfterBorder].Context.BeginTime > newMinute) {
                // Border を過去に移動 -> 直後のアクティビティを過去に伸ばす
                Acts.OverwriteBeginTime(indexJustAfterBorder, newMinute);
            }
            else if (Acts[indexJustAfterBorder].Context.BeginTime < newMinute) {
                // Border を未来に移動 -> 直前のアクティビティを未来に伸ばす
                Acts.OverwriteEndTime(indexJustAfterBorder - 1, newMinute);
            }
        }
        #endregion
    }

    public class ActivitiesMgr4Test : ActivitiesMgr {
        public void BeginNewAct(IProject proj, string name, bool isEffective, MinuteOfDay beginTime) {
            Acts.PushBack(new Activity(new Content(proj, name, isEffective), new Context(beginTime)));
        }
        public void BeginNewAct(IProject proj, string name, MinuteOfDay beginTime) {
            if (Acts.Count > 0) {
                Debug.LogError($"BeginNewActBefore[{Acts.Back.Context.BeginTime}><{beginTime}]");
            }
            Acts.PushBack(new Activity(new Content(proj, name), new Context(beginTime)));
            Debug.LogError($"BeginNewActAfter[{Acts.Back.Context.BeginTime}><{beginTime}]");
        }
    }

}
