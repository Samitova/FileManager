using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace FileManager.ViewModel.Commands
{

    public static class EventBindings
    {
        /// <summary>
        /// The Event Bindings Property
        /// </summary>
        private static readonly DependencyProperty EventBindingsProperty =
          DependencyProperty.RegisterAttached("EventBindings", typeof(EventBindingCollection), typeof(EventBindings),
          new PropertyMetadata(null, new PropertyChangedCallback(OnEventBindingsChanged)));

        /// <summary>
        /// Gets the event bindings
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static EventBindingCollection GetEventBindings(DependencyObject o)
        {
            return (EventBindingCollection)o.GetValue(EventBindingsProperty);
        }

        /// <summary>
        /// Sets the event bindings
        /// </summary>
        /// <param name="o"></param>
        /// <param name="value"></param>
        public static void SetEventBindings(DependencyObject o, EventBindingCollection value)
        {
            o.SetValue(EventBindingsProperty, value);
        }

        /// <summary>
        /// Invoked when event bindings changed
        /// </summary>
        /// <param name="o"></param>
        /// <param name="args"></param>
        public static void OnEventBindingsChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            EventBindingCollection oldEventBindings = args.OldValue as EventBindingCollection;
            EventBindingCollection newEventBindings = args.NewValue as EventBindingCollection;

            //  If we have new set of event bindings, binds each one.
            if (newEventBindings != null)
            {
                foreach (EventBinding binding in newEventBindings)
                {
                    binding.Bind(o);
                }
            }
        }
    }
}
