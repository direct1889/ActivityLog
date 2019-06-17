using System.Collections.Generic;
using UnityEngine;
using static du.Ex.ExCollection;
using static du.Ex.ExList;

namespace Main.Act {

    /// <summary> アクティビティの系列(日毎) </summary>
    public class ActRecordSequence : IActRecordSequence {
        #region field
        IList<IActRecord> m_acts = new List<IActRecord>();
        #endregion

        #region getter
        public IROActRecord this [int index] => m_acts[index];
        public IROActRecord this [MinuteOfDay time] => m_acts[IndexOf(time)];
        public int Count => m_acts.Count;
        public IROActRecord Back => m_acts.Back();

        public int IndexOf(MinuteOfDay time) {
            if (Sys.Chronos.Now < time) {
                throw new System.ArgumentException("Argument is future.");
            }
            for (int i = m_acts.Count - 1; i >= 0; --i) {
                if (m_acts[i].Context.BeginTime <= time) { return i; }
            }
            // ここには本来到達し得ない
            throw new System.Exception("");
        }
        #endregion

        #region public
        public void PushBack(IActRecord act) {
            if (m_acts.Count == 0) { m_acts.Add(act); }
            else if (Back.Context.BeginTime < act.Context.BeginTime) {
                m_acts.Back().MutableContext.ResetFollowAct(act);
                m_acts.Add(act);
            }
            else {
                for (int i = Count - 1; i > IndexOf(act.Context.BeginTime); --i) {
                    RemoveAt(i);
                }
                PushBack(act);
            }
        }
        public void Insert(MinuteOfDay begin, MinuteOfDay end, IActivity cnt) {
            IIndependentActRecord act = new IndependentActRecord(cnt, new IndependentContext(begin, end));
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
            if (!m_acts.IsValidIndex(index)) { return; }
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
            if (newEnd != null) { Insert(newBegin, (MinuteOfDay)newEnd, act.Activity); }
            else { PushBack(act); }
        }
        public void OverwriteCnt(int index, IActivity newContent) {
            m_acts[index].ResetAct(newContent);
        }
        public void OverwriteBeginTime(int index, MinuteOfDay newBegin) {
            Move(index, newBegin, m_acts[index].Context.EndTime);
        }
        public void OverwriteEndTime(int index, MinuteOfDay? newEnd) {
            Move(index, m_acts[index].Context.BeginTime, newEnd);
        }
        #endregion
    }

}
