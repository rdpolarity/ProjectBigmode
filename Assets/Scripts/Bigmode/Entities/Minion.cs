using Unity.VisualScripting;

namespace Bigmode {
    public class Minion : Entity, IFriendly {
        public MinionType type;

        public IMinionCountChangeListener minionCountChangeListener;

        public override void Die()
        {
            base.Die();
            if (minionCountChangeListener != null)
                minionCountChangeListener.MinionCountChanged(-1);
        }

    }
}