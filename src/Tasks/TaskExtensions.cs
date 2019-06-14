using System;
using System.Threading.Tasks;

namespace HLib.Tasks
{
    public static class TaskExtensions
    {
        public static async Task<T> Then<T>(this Task task, Func<Task<T>> f)
        {
            await task;
            return await f();
        }
        public static async Task Then(this Task task, Func<Task> f)
        {
            await task;
            await f();
        }
        public static async Task<U> Then<T, U>(this Task<T> task, Func<T, Task<U>> f) => await f(await task);
        public static async Task Then<T>(this Task<T> task, Func<T, Task> f) => await f(await task);
    }
}
