using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlan_Desktop
{
    public class WeakEventHandler<TInstance, TSender, TEventArgs>
        where TInstance : class
    {
        private readonly WeakReference _instanceReference;
        private readonly Action<TInstance, TSender, TEventArgs> _handlerAction;
        private Action<WeakEventHandler<TInstance, TSender, TEventArgs>> _detachAction;

        /// <summary>
        /// Initializes a new instance of the WeakEventHandler{TInstance, TSender, TEventArgs} class.
        /// </summary>
        /// <param name="instance">The object with the actual handler, to which a weak reference will be held.</param>
        /// <param name="handlerAction">An action to invoke the actual handler.</param>
        /// <param name="detachAction">An action to detach the weak handler from the event.</param>
        public WeakEventHandler(
            TInstance instance,
            Action<TInstance, TSender, TEventArgs> handlerAction,
            Action<WeakEventHandler<TInstance, TSender, TEventArgs>> detachAction)
        {
            this._instanceReference = new WeakReference(instance);
            this._handlerAction = handlerAction;
            this._detachAction = detachAction;
        }

        /// <summary>
        /// Removes the weak event handler from the event.
        /// </summary>
        public void Detach()
        {
            if (this._detachAction != null)
            {
                this._detachAction(this);
                this._detachAction = null;
            }
        }

        /// <summary>
        /// Invokes the handler action.
        /// </summary>
        /// <remarks>
        /// This method must be added as the handler to the event.
        /// </remarks>
        /// <param name="sender">The event source object.</param>
        /// <param name="e">The event arguments.</param>
        public void OnEvent(TSender sender, TEventArgs e)
        {
            var instance = this._instanceReference.Target as TInstance;
            if (instance != null)
            {
                if (this._handlerAction != null)
                {
                    this._handlerAction((TInstance)this._instanceReference.Target, sender, e);
                }
            }
            else
            {
                this.Detach();
            }
        }
    }
}
