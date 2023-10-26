using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002DD RID: 733
    [DebuggerStepThrough]
    public abstract class TaskUpdater<TTask>
    {
        // Token: 0x17000159 RID: 345
        // (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0002C5E4 File Offset: 0x0002A7E4
        private IEnumerable<TaskUpdater<TTask>.TaskInfo> AllTasks
        {
            get
            {
                return this.ActiveTasks.Concat(this._queuedTasks);
            }
        }

        // Token: 0x1700015A RID: 346
        // (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0002C5F8 File Offset: 0x0002A7F8
        private IEnumerable<TaskUpdater<TTask>.TaskInfo> ActiveTasks
        {
            get
            {
                return this._tasks;
            }
        }

        // Token: 0x06000FB5 RID: 4021 RVA: 0x0002C600 File Offset: 0x0002A800
        public void AddTask(TTask task, int priority)
        {
            this.AddTaskInternal(task, priority);
        }

        // Token: 0x06000FB6 RID: 4022 RVA: 0x0002C60C File Offset: 0x0002A80C
        private void AddTaskInternal(TTask task, int priority)
        {
            ModestTree.Assert.That(!(from x in this.AllTasks
                                     select x.Task).ContainsItem(task), "Duplicate task added to DependencyRoot with name '" + task.GetType().FullName + "'");
            this._queuedTasks.Add(new TaskUpdater<TTask>.TaskInfo(task, priority));
        }

        // Token: 0x06000FB7 RID: 4023 RVA: 0x0002C684 File Offset: 0x0002A884
        public void RemoveTask(TTask task)
        {
            TaskUpdater<TTask>.TaskInfo taskInfo = (from x in this.AllTasks
                                                    select x).SingleOrDefault<TaskUpdater<TTask>.TaskInfo>();
            ModestTree.Assert.IsNotNull(taskInfo, "Tried to remove a task not added to DependencyRoot, task = " + task.GetType().Name);
            ModestTree.Assert.That(!taskInfo.IsRemoved, "Tried to remove task twice, task = " + task.GetType().Name);
            taskInfo.IsRemoved = true;
        }

        // Token: 0x06000FB8 RID: 4024 RVA: 0x0002C714 File Offset: 0x0002A914
        public void OnFrameStart()
        {
            this.AddQueuedTasks();
        }

        // Token: 0x06000FB9 RID: 4025 RVA: 0x0002C71C File Offset: 0x0002A91C
        public void UpdateAll()
        {
            this.UpdateRange(int.MinValue, int.MaxValue);
        }

        // Token: 0x06000FBA RID: 4026 RVA: 0x0002C730 File Offset: 0x0002A930
        public void UpdateRange(int minPriority, int maxPriority)
        {
            LinkedListNode<TaskUpdater<TTask>.TaskInfo> next;
            for (LinkedListNode<TaskUpdater<TTask>.TaskInfo> linkedListNode = this._tasks.First; linkedListNode != null; linkedListNode = next)
            {
                next = linkedListNode.Next;
                TaskUpdater<TTask>.TaskInfo value = linkedListNode.Value;
                if (!value.IsRemoved && value.Priority >= minPriority && (maxPriority == 2147483647 || value.Priority < maxPriority))
                {
                    this.UpdateItem(value.Task);
                }
            }
            this.ClearRemovedTasks(this._tasks);
        }

        // Token: 0x06000FBB RID: 4027 RVA: 0x0002C798 File Offset: 0x0002A998
        private void ClearRemovedTasks(LinkedList<TaskUpdater<TTask>.TaskInfo> tasks)
        {
            LinkedListNode<TaskUpdater<TTask>.TaskInfo> next;
            for (LinkedListNode<TaskUpdater<TTask>.TaskInfo> linkedListNode = tasks.First; linkedListNode != null; linkedListNode = next)
            {
                next = linkedListNode.Next;
                if (linkedListNode.Value.IsRemoved)
                {
                    tasks.Remove(linkedListNode);
                }
            }
        }

        // Token: 0x06000FBC RID: 4028 RVA: 0x0002C7CC File Offset: 0x0002A9CC
        private void AddQueuedTasks()
        {
            for (int i = 0; i < this._queuedTasks.Count; i++)
            {
                TaskUpdater<TTask>.TaskInfo taskInfo = this._queuedTasks[i];
                if (!taskInfo.IsRemoved)
                {
                    this.InsertTaskSorted(taskInfo);
                }
            }
            this._queuedTasks.Clear();
        }

        // Token: 0x06000FBD RID: 4029 RVA: 0x0002C818 File Offset: 0x0002AA18
        private void InsertTaskSorted(TaskUpdater<TTask>.TaskInfo task)
        {
            for (LinkedListNode<TaskUpdater<TTask>.TaskInfo> linkedListNode = this._tasks.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
            {
                if (linkedListNode.Value.Priority > task.Priority)
                {
                    this._tasks.AddBefore(linkedListNode, task);
                    return;
                }
            }
            this._tasks.AddLast(task);
        }

        // Token: 0x06000FBE RID: 4030
        protected abstract void UpdateItem(TTask task);

        // Token: 0x06000FC0 RID: 4032 RVA: 0x0002C88C File Offset: 0x0002AA8C
        [Preserve]
        private static InjectTypeInfo __zenCreateInjectTypeInfo()
        {
            return new InjectTypeInfo(typeof(TaskUpdater<TTask>), new InjectTypeInfo.InjectConstructorInfo(null, new InjectableInfo[0]), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
        }

        // Token: 0x040004F4 RID: 1268
        private readonly LinkedList<TaskUpdater<TTask>.TaskInfo> _tasks = new LinkedList<TaskUpdater<TTask>.TaskInfo>();

        // Token: 0x040004F5 RID: 1269
        private readonly List<TaskUpdater<TTask>.TaskInfo> _queuedTasks = new List<TaskUpdater<TTask>.TaskInfo>();

        // Token: 0x020002DE RID: 734
        private class TaskInfo
        {
            // Token: 0x06000FC1 RID: 4033 RVA: 0x0002C8D0 File Offset: 0x0002AAD0
            public TaskInfo(TTask task, int priority)
            {
                this.Task = task;
                this.Priority = priority;
            }

            // Token: 0x06000FC2 RID: 4034 RVA: 0x0002C8E8 File Offset: 0x0002AAE8
            private static object __zenCreate(object[] P_0)
            {
                return new TaskUpdater<TTask>.TaskInfo((TTask)((object)P_0[0]), (int)P_0[1]);
            }

            // Token: 0x06000FC3 RID: 4035 RVA: 0x0002C918 File Offset: 0x0002AB18
            [Preserve]
            private static InjectTypeInfo __zenCreateInjectTypeInfo()
            {
                return new InjectTypeInfo(typeof(TaskUpdater<TTask>.TaskInfo), new InjectTypeInfo.InjectConstructorInfo(new ZenFactoryMethod(TaskUpdater<TTask>.TaskInfo.__zenCreate), new InjectableInfo[]
                {
                    new InjectableInfo(false, null, "task", typeof(TTask), null, InjectSources.Any),
                    new InjectableInfo(false, null, "priority", typeof(int), null, InjectSources.Any)
                }), new InjectTypeInfo.InjectMethodInfo[0], new InjectTypeInfo.InjectMemberInfo[0]);
            }

            // Token: 0x040004F6 RID: 1270
            public TTask Task;

            // Token: 0x040004F7 RID: 1271
            public int Priority;

            // Token: 0x040004F8 RID: 1272
            public bool IsRemoved;
        }
    }
}
