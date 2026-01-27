using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class CoroAwaitable {
    public static Task AwaitWhile(MonoBehaviour obj, Func<bool> cond) {
        return Make(obj, AwaitWhileCoro(cond));
    }
    private static IEnumerator AwaitWhileCoro(Func<bool> cond) {
        while (cond.Invoke()) yield return null;
    }

    public static Task Make(MonoBehaviour obj, string method) {
        var tcs = new TaskCompletionSource<bool>();
        obj.StartCoroutine(AwaitCompletion(obj, method, tcs));
        return tcs.Task;
    }
    public static Task Make(MonoBehaviour obj, IEnumerator coro) {
        var tcs = new TaskCompletionSource<bool>();
        obj.StartCoroutine(AwaitCompletion(coro, tcs));
        return tcs.Task;
    }

    private static IEnumerator AwaitCompletion(MonoBehaviour obj, string method, TaskCompletionSource<bool> tcs) {
        yield return obj.StartCoroutine(method);
        tcs.SetResult(true);
    }
    private static IEnumerator AwaitCompletion(IEnumerator coro, TaskCompletionSource<bool> tcs) {
        yield return coro;
        tcs.SetResult(true);
    }
}

public class TaskCoro {
    public static IEnumerator Make(Task task) {
        while (!task.IsCompleted)
            yield return null;
        if (task.IsFaulted)
            UnityEngine.Debug.LogError(task.Exception.ToString());
    }
}

public static class ManhattanDistanceExt
{
    public static int ManhattanDistance(this Vector3Int a, Vector3Int b)
        => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
}
