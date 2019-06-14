using Main.Act;

namespace Dev {

    public class ActSequenceMgrBasedOnVirtualChronos : ActSequenceMgr {
        public override void BeginNewAct(IActivity act) {
            Acts.PushBack(new ActRecord(act, Dev.VirtualChronos.Now));
        }
        public override void BeginNewAct(IProject proj, string name, bool isEffective) {
            Acts.PushBack(new ActRecord(proj, name, isEffective, VirtualChronos.Now));
        }
        public override void BeginNewAct(IProject proj, string name) {
            Acts.PushBack(new ActRecord(proj, name, VirtualChronos.Now));
        }
    }

}
