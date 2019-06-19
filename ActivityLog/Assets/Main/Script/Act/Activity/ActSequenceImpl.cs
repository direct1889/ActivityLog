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
        public string Dump() {
            string s = "";
            for (int i = 0; i < m_acts.Count; ++i) {
                s += $"// {m_acts[i].Context}------------\n";
                s += m_acts[i].Activity + "\n";
            }
            s += m_acts.Back().Context.EndTime?.ToString() ?? $"// cont. ~~~~~~~~~~~~\n";
            return s;
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
            m_acts[index].ResetAct(null);
            m_acts.RemoveAt(index);
        }
        public void Move(int index, MinuteOfDay newBegin, MinuteOfDay? newEnd) {
            var act = m_acts[index];
            Debug.LogError($"{act} --MOVE--> [{newBegin}, {newEnd}]");
            RemoveAt(index);
            if (newEnd is null) { PushBack(act); } // 継続中 ∴PushBack
            else { Insert(newBegin, (MinuteOfDay)newEnd, act.Activity); }
        }
        public void OverwriteCnt(int index, IActivity newContent) {
            m_acts[index].ResetAct(newContent);
        }
        public void OverwriteBeginTime(int index, MinuteOfDay newBegin) {
            IActRecord act = m_acts[index];
            int justBefore = IndexOf(newBegin);
            // e.g.        |------ NEW ---------------|
            // |- justBefore -|- In0 -|- In1 -|- OLD -|-- justAfter --| の場合

            // (1)包含アクティビティを削除
            // e.g.        |------ NEW ---------------|
            // |- justBefore -|               |- OLD -|-- justAfter --| の場合
            for (int i = index - 1; i > justBefore; --i) {
                RemoveAt(i); // 後ろから消さないとindexがずれる
            }
            // (2)直前のアクティビティ -> Follow を書き換え
            // e.g.        |------ NEW ---------------|
            // |- justBefore -----------------|- OLD -|-- justAfter --| の場合
            m_acts[justBefore].MutableContext.ResetFollowAct(act);
            // (3)変更対象のアクティビティ -> Begin を書き換え
            // e.g.        |------ NEW ---------------|
            // |- jB ------|-------------------- OLD -|-- justAfter --| の場合
            act.MutableContext.ResetPrecedeAct(newBegin);
        }
        public void OverwriteEndTime(int index, MinuteOfDay? newEnd) {
            // Move(index, m_acts[index].Context.BeginTime, newEnd);
            if (newEnd is null) {}
            MinuteOfDay newEnd0 = (MinuteOfDay)newEnd;
            IActRecord act = m_acts[index];
            int justAfter = IndexOf(newEnd0);
            // e.g.           |------ NEW ---------------|
            // |- justBefore -|- OLD -|- In0 -|- In1 -|-- justAfter --| の場合

            // (1)包含アクティビティを削除
            // e.g.           |------ NEW ---------------|
            // |- justBefore -|- OLD -|               |-- justAfter --| の場合
            for (int i = justAfter - 1; i > index; --i) {
                RemoveAt(i); // 後ろから消さないとindexがずれる
            }
            justAfter = index + 1;
            // (2)直後のアクティビティ -> Precede を書き換え
            // e.g.           |------ NEW ---------------|
            // |- justBefore -|- OLD -|                  |---- jA ----| の場合
            m_acts[justAfter].MutableContext.ResetPrecedeAct(newEnd0);
            // (2)変更対象のアクティビティ -> Follow を書き換え
            // e.g.           |------ NEW ---------------|
            // |- justBefore -|- OLD --------------------|---- jA ----| の場合
            act.MutableContext.ResetFollowAct(m_acts[justAfter]);
        }
        #endregion
    }

}
