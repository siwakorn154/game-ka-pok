using UnityEngine;

// ทุก Item Action ต้อง inherit จากนี้
public abstract class ItemAction : ScriptableObject
{
    // ตอนหยิบขึ้นมาถือ
    public virtual void OnEquip(PlayerContext player) { }

    // ทุก Frame ที่ถืออยู่ (ถ้าต้องการ)
    public virtual void OnHold(PlayerContext player) { }

    // ตอนใช้งาน (คลิกซ้าย)
    public virtual void OnUse(PlayerContext player) { }

    // ตอนเปลี่ยนช่องหรือวางลง
    public virtual void OnUnequip(PlayerContext player) { }
}