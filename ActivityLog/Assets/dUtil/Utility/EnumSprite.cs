using UnityEngine;
using System.Collections.Generic;

namespace du.Misc {

    public class EnumSprite<EnumType> where EnumType : struct {
        #region field
        IDictionary<EnumType, Sprite> m_sprites
            = new Dictionary<EnumType, Sprite>();
        #endregion

        #region ctor/dtor
        public EnumSprite(string filePath, bool isEliminateLastElement = false) {
            Sprite[] rawSprites = Resources.LoadAll<Sprite>(filePath);
            int max = System.Enum.GetValues(typeof(EnumType)).Length;
            if (isEliminateLastElement) { --max; }

            for (int i = 0; i < max; ++i) {
                m_sprites.Add(
                    (EnumType)System.Enum.ToObject(typeof(EnumType), i),
                    rawSprites[i]
                    );
            }
        }
        #endregion

        #region getter
        public Sprite this[EnumType type] => m_sprites[type];
        #endregion
    }

}
