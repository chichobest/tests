using System;
using System.Collections.Generic;
using System.Linq;
using NNSalesApp.Core.Interfaces;
using NN_CS_iPad_CalculateManager.clases.EB;
using Xamarin.Auth;


namespace NNSalesApp.Core.Services
{
   public class NNPasswordManager : IPasswordManager
   {
      private AccountStore vault;
      private Account account;
      private static string AppName => "NNSalesApp";
      
      public NNPasswordManager()
      {
         vault =AccountStore.Create();
      }

      /// <summary>
      /// Adds a credential to the Credential Locker.
      /// </summary>
      /// <param name="resource">The resource for which the credentials are used.</param>
      /// <param name="userName">The user name that must be present in the credentials.</param>
      /// <param name="password">The password for the created credentials.</param>
      /// <exception cref="ArgumentNullException">Dispatched when <paramref name="resource"/>, <paramref name="userName"/> or
      /// <paramref name="password"/> is <c>Null</c>
      /// </exception>
      public bool CreateAccount(string resource, string userName, string password)
      {
         if (string.IsNullOrEmpty(resource))
            throw new ArgumentNullException(nameof(resource), "The argument is null");
         if (string.IsNullOrEmpty(userName))
            throw new ArgumentNullException(nameof(userName), "The argument is null");
         if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "The argument is null");

         try
         {
            account = new Account {Username = userName};
            account.Properties.Add(resource,password);
            vault.Save(account,AppName);

            var lista = vault.FindAccountsForService(AppName);
            
            return true;
         }
         catch (Exception e)
         {
            return false;
         }
      }      

      /// <summary>
        /// Adds a credential to the Credential Locker.
        /// </summary>
        /// <param name="resource">The resource for which the credentials are used.</param>
        /// <param name="password">The password for the created credentials.</param>
        /// <exception cref="ArgumentNullException">Dispatched when <paramref name="resource"/> or
        /// <paramref name="password"/> is <c>Null</c>
        /// </exception>
         /*
        public bool Add(string resource, string password)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentNullException(nameof(resource), "The argument is null");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "The argument is null");

            try
            {
                
                account.Properties.Add(resource, password);
                vault.Save(account, AppName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }*/
  
        public bool Add(string username, string resource, string password)
        {
            if (string.IsNullOrEmpty(resource))
                throw new ArgumentNullException(nameof(resource), "The argument is null");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "The argument is null");

            try
            {
                var lista = vault.FindAccountsForService(AppName);
                account.Properties.Add(resource, password);
                vault.Save(account, AppName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

      /// <summary>
      /// Searches the Credential Locker for credentials matching the resource specified.
      /// </summary>
      /// <param name="resource"> When this method returns, contains an IVectorView of credential objects that match the search criteria.</param>
      /// <exception cref="ArgumentNullException">Dispatched when <paramref name="resource"/> is <c>Null</c></exception>
      /// <returns>The resource to be searched for.</returns>
      public IReadOnlyList<Account> FindAllByResource(string resource)
      {
         if (string.IsNullOrEmpty(resource))
            throw new ArgumentNullException(nameof(resource), "The argument is null");

         try
         {
            return vault.FindAccountsForService(AppName).Where(s=>s.Properties.ContainsKey(resource)).ToList();
         }
         catch (Exception)
         {
            return null;
         }
      }

      /// <summary>Searches the Credential Locker for credentials that match the user name specified.</summary>
      /// <returns>When this method returns, contains an IVectorView of credential objects that match the search criteria.</returns>
      /// <exception cref="ArgumentNullException">Dispatched when <paramref name="userName"/> is <c>Null</c></exception>
      /// <param name="userName">The user name to be searched for.</param>
      public IReadOnlyList<Account> FindAllByUserName(string userName)
      {
         if (string.IsNullOrEmpty(userName))
            throw new ArgumentNullException(nameof(userName), "The argument is null");

         try
         {
            return vault.FindAccountsForService(AppName).Where(s => s.Username.Contains(userName)).ToList();
         }
         catch (Exception)
         {
            return null;
         }
      }

      /// <summary>Reads a credential from the Credential Locker.</summary>
      /// <returns>The returned credential that contains all the data.</returns>
      /// <param name="resource">The resource for which the credential is used.</param>
      /// <param name="userName">The user name that must be present in the credential.</param>
      /// <exception cref="ArgumentNullException">Dispatched when <paramref name="resource"/> or <paramref name="userName"/> is <c>Null</c>
      /// </exception>
      public Account Retrieve(string resource, string userName)
      {
         if (string.IsNullOrEmpty(resource))
            throw new ArgumentNullException(nameof(resource), "The argument is null");
         if (string.IsNullOrEmpty(userName))
            throw new ArgumentNullException(nameof(userName), "The argument is null");

         try
         {
            return vault.FindAccountsForService(AppName).FirstOrDefault(s => s.Username.Contains(userName));
         }
         catch (Exception)
         {
            return null;
         }
      }

      /// <summary>Reads a credential from the Credential Locker.</summary>
      /// <returns>The returned credential that contains all the data.</returns>
      /// <param name="resource">The resource for which the credential is used.</param>
      /// <exception cref="ArgumentNullException">Dispatched when <paramref name="resource"/> is <c>Null</c></exception>
      public Account Retrieve(string resource)
      {
         if (string.IsNullOrEmpty(resource))
            throw new ArgumentNullException(nameof(resource), "The argument is null");

         try
         {
            return vault.FindAccountsForService(AppName).FirstOrDefault(s => s.Properties.ContainsKey(resource));
         }
         catch (Exception)
         {
            return null;
         }
      }
/// <summary>
/// 
/// </summary>
/// <param name="resource"></param>
/// <returns></returns>
/// <exception cref="ArgumentNullException"></exception>
      public string RetrievePass(string resource)
      {
         if (string.IsNullOrEmpty(resource))
            throw new ArgumentNullException(nameof(resource), "The argument is null");

         try
         {
				return account.Properties.FirstOrDefault(p => p.Key.Contains(resource)).Value;
         }
         catch (Exception)
         {
            return String.Empty;
         }
      }

      /// <summary>Retrieves all of the credentials stored in the Credential Locker.</summary>
      /// <returns>When this method returns, contains an IVectorView output of credential objects that match the search criteria. This output is a snapshot and not dynamic. If the results are used for updating or deleting credentials, those changes won't be reflected in the previous output.</returns>
      public IReadOnlyList<Account> RetrieveAll()
      {
         try
         {
            return vault.FindAccountsForService(AppName).ToList();
         }
         catch (Exception)
         {
            return null;
         }

      }

      /// <summary>
      /// Removes a credential from the Credential Locker.
      /// </summary>
      /// <param name="resource">The resource for which the credentials are used.</param>
      /// <param name="userName">The user name that must be present in the credentials.</param>
      /// <param name="password">The password for the created credentials.</param>
      /// <exception cref="ArgumentNullException">Dispatched when <paramref name="resource"/>, <paramref name="userName"/> or
      /// <paramref name="password"/> is <c>Null</c>
      /// </exception>
      public bool Remove(string resource, string userName, string password)
      {
         if (string.IsNullOrEmpty(resource))
            throw new ArgumentNullException(nameof(resource), "The argument is null");
         if (string.IsNullOrEmpty(userName))
            throw new ArgumentNullException(nameof(userName), "The argument is null");
         if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "The argument is null");

         try
         {
            Account account = new Account {Username = userName};
            account.Properties.Add(resource,password);
            vault.Delete(account,AppName);
            return true;
         }
         catch (Exception)
         {
            return false;
         }
      }

      /// <summary>
      /// Removes a credential from the Credential Locker.
      /// </summary>
      /// <param name="resource">The resource for which the credentials are used.</param>
      /// <param name="password">The password for the created credentials.</param>
      /// <exception cref="ArgumentNullException">Dispatched when <paramref name="resource"/> or
      /// <paramref name="password"/> is <c>Null</c>
      /// </exception>
      public bool Remove(string resource, string password)
      {
         if (string.IsNullOrEmpty(resource))
            throw new ArgumentNullException(nameof(resource), "The argument is null");
         if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "The argument is null");

         try
         {
            Account account = new Account {Username = resource};
            account.Properties.Add(resource,password);
            vault.Delete(account,AppName);
            return true;
         }
         catch (Exception)
         {
            return false;
         }
      }

      /// <summary>
      /// Removes all credentials from the Credential Locker.
      /// </summary>
      public bool RemoveAll()
      {
         try
         {
            
            foreach (var passwordCredential in vault.FindAccountsForService(AppName))
            {
               vault.Delete(passwordCredential,AppName);
            }

            return true;
         }
         catch (Exception)
         {
            return false;
         }
      }
   }
}

