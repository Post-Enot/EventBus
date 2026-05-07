using UnityEngine;
using UnityEngine.InputSystem;

namespace PostEnot.Toolkits.EventManagement.Input
{
    public abstract class InputActionEventAdapter : MonoBehaviour
    {
        protected IInputActionCollection2 InputActions { get; private set; }
        protected IEventInvoker Invoker { get; private set; }

        public void Init(IInputActionCollection2 inputActions, IEventBus eventBus)
        {
            InputActions = inputActions;
            Invoker = eventBus.CreateInvoker();
        }

        public static void AdaptInputActions<TInputActionEventAdapter>(
            IInputActionCollection2 inputActions,
            IEventBus eventBus,
            bool dontDestroyOnLoad = false,
            string gameObjectName = default,
            Transform root = null)
            where TInputActionEventAdapter : InputActionEventAdapter
        {
            if (string.IsNullOrEmpty(gameObjectName))
            {
                gameObjectName = "Input Actions Event Adapter";
            }
            GameObject adapterGameObject = new(gameObjectName);
            TInputActionEventAdapter adapter = adapterGameObject.AddComponent<TInputActionEventAdapter>();
            adapter.Init(inputActions, eventBus);
            if (root != null)
            {
                adapterGameObject.transform.parent = root;
            }
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(adapterGameObject);
            }
        }
    }
}
