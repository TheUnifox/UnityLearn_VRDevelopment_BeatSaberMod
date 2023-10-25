using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x0200004B RID: 75
    [NoReflectionBaking]
    public class GameObjectCreationParameters
    {
        // Token: 0x17000030 RID: 48
        // (get) Token: 0x060001F4 RID: 500 RVA: 0x00006BC4 File Offset: 0x00004DC4
        // (set) Token: 0x060001F5 RID: 501 RVA: 0x00006BCC File Offset: 0x00004DCC
        public string Name { get; set; }

        // Token: 0x17000031 RID: 49
        // (get) Token: 0x060001F6 RID: 502 RVA: 0x00006BD8 File Offset: 0x00004DD8
        // (set) Token: 0x060001F7 RID: 503 RVA: 0x00006BE0 File Offset: 0x00004DE0
        public string GroupName { get; set; }

        // Token: 0x17000032 RID: 50
        // (get) Token: 0x060001F8 RID: 504 RVA: 0x00006BEC File Offset: 0x00004DEC
        // (set) Token: 0x060001F9 RID: 505 RVA: 0x00006BF4 File Offset: 0x00004DF4
        public Transform ParentTransform { get; set; }

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x060001FA RID: 506 RVA: 0x00006C00 File Offset: 0x00004E00
        // (set) Token: 0x060001FB RID: 507 RVA: 0x00006C08 File Offset: 0x00004E08
        public Func<InjectContext, Transform> ParentTransformGetter { get; set; }

        // Token: 0x17000034 RID: 52
        // (get) Token: 0x060001FC RID: 508 RVA: 0x00006C14 File Offset: 0x00004E14
        // (set) Token: 0x060001FD RID: 509 RVA: 0x00006C1C File Offset: 0x00004E1C
        public Vector3? Position { get; set; }

        // Token: 0x17000035 RID: 53
        // (get) Token: 0x060001FE RID: 510 RVA: 0x00006C28 File Offset: 0x00004E28
        // (set) Token: 0x060001FF RID: 511 RVA: 0x00006C30 File Offset: 0x00004E30
        public Quaternion? Rotation { get; set; }

        // Token: 0x06000200 RID: 512 RVA: 0x00006C3C File Offset: 0x00004E3C
        public override int GetHashCode()
        {
            return (((((17 * 29 + ((this.Name == null) ? 0 : this.Name.GetHashCode())) * 29 + ((this.GroupName == null) ? 0 : this.GroupName.GetHashCode())) * 29 + ((this.ParentTransform == null) ? 0 : this.ParentTransform.GetHashCode())) * 29 + ((this.ParentTransformGetter == null) ? 0 : this.ParentTransformGetter.GetHashCode())) * 29 + ((this.Position == null) ? 0 : this.Position.Value.GetHashCode())) * 29 + ((this.Rotation == null) ? 0 : this.Rotation.Value.GetHashCode());
        }

        // Token: 0x06000201 RID: 513 RVA: 0x00006D20 File Offset: 0x00004F20
        public override bool Equals(object other)
        {
            return other is GameObjectCreationParameters && (GameObjectCreationParameters)other == this;
        }

        // Token: 0x06000202 RID: 514 RVA: 0x00006D38 File Offset: 0x00004F38
        public bool Equals(GameObjectCreationParameters that)
        {
            return this == that;
        }

        // Token: 0x06000203 RID: 515 RVA: 0x00006D44 File Offset: 0x00004F44
        public static bool operator ==(GameObjectCreationParameters left, GameObjectCreationParameters right)
        {
            return object.Equals(left.Name, right.Name) && object.Equals(left.GroupName, right.GroupName);
        }

        // Token: 0x06000204 RID: 516 RVA: 0x00006D6C File Offset: 0x00004F6C
        public static bool operator !=(GameObjectCreationParameters left, GameObjectCreationParameters right)
        {
            return !left.Equals(right);
        }

        // Token: 0x040000C3 RID: 195
        public static readonly GameObjectCreationParameters Default = new GameObjectCreationParameters();
    }
}
