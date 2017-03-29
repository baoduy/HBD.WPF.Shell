#region

using System;
using System.Threading.Tasks;

#endregion

namespace HBD.WPF.Common
{
    public static class TaskCreater
    {
        public static Task CreateTask(Action action) => Task.Run(action);

        public static Task CreateTask<T>(Action<T> action, T parameter)
        {
            var task = new TaskCompletionSource<object>();
            try
            {
                action.Invoke(parameter);
                task.SetResult(null);
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }
            return task.Task;
        }

        public static Task<T> CreateTask<T>(Func<T> action)
        {
            var task = new TaskCompletionSource<T>();
            try
            {
                task.SetResult(action.Invoke());
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }
            return task.Task;
        }

        public static Task<TResult> CreateTask<TParameter, TResult>(Func<TParameter, TResult> action,
            TParameter parameter)
        {
            var task = new TaskCompletionSource<TResult>();
            try
            {
                task.SetResult(action.Invoke(parameter));
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }
            return task.Task;
        }
    }
}