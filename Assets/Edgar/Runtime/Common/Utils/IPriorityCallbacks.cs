using System.Collections.Generic;

namespace Edgar.Unity
{
    public interface IPriorityCallbacks<TCallback>
    {
        void RegisterCallback(int priority, TCallback callback, bool replaceExisting = false);

        void RegisterCallbackAfter(int priority, TCallback callback, bool replaceExisting = false);

        void RegisterCallbackBefore(int priority, TCallback callback, bool replaceExisting = false);

        void RegisterCallbackInsteadOf(int priority, TCallback callback);

        void RegisterBeforeAll(TCallback callback);

        void RegisterAfterAll(TCallback callback);

        List<TCallback> GetCallbacks();
    }
}