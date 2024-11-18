using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Auth
{
    /// <summary>
    /// Interface for securely storing data.
    /// </summary>
    public interface ISecureStorage
    {
        /// <summary>
        /// Required for Android Only, other platforms have implicit secure storage.
        /// </summary>
        /// <param name="password"></param>
        bool UnlockSecureStorage(string password);

        /// <summary>
        /// Stores data.
        /// </summary>
        /// <param name="key">Key for the data.</param>
        /// <param name="dataBytes">Data bytes to store.</param>
        void Store(string key, string value);

        /// <summary>
        /// If the user can't remember the password, they can reset the storage and enter a new one, this will remove any old values
        /// </summary>
        /// <param name="newPassword"></param>
        void Reset(string newPassword);

        /// <summary>
        /// Returns true if secure storage has been setup w/ a password and false if not.
        /// </summary>
        bool IsSetup { get; }

        /// <summary>
        /// Return a key from secure storage.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Retrieve(string key);

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="key">Key for the data to be deleted.</param>
        void Delete(string key);

        /// <summary>
        /// Checks if the storage contains a key.
        /// </summary>
        /// <param name="key">The key to search.</param>
        /// <returns>True if the storage has the key, otherwise false.</returns>
        bool Contains(string key);

        /// <summary>
        /// Will always be true for iOS/UWP, for Android, this will be true after UnlockSecureStorage has been called.
        /// </summary>
        bool IsUnlocked { get; }
    }
}
