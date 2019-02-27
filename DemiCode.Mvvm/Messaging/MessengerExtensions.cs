using System;
using System.Linq;
using System.Collections.Generic;

namespace DemiCode.Mvvm.Messaging
{
    /// <summary>
    /// Helper methods extending <see cref="IMessenger"/>.
    /// </summary>
    public static class MessengerExtensions
    {

         /// <summary>
         /// Get all currently registered recipients of type <typeparamref name="TRecipientType"/>.
         /// </summary>
         public static IEnumerable<TRecipientType> Registrations<TRecipientType>(this IMessenger messenger)
         {
             var impl = messenger as Messenger;
             if (impl == null)
                 throw new ArgumentException();

             return impl._recipientsStrictAction.Values.SelectMany(x => x)
                 .Select(y => y.Action.Target)
                 .OfType<TRecipientType>();
         }
    }
}