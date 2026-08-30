using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectionHelper {
   public static Type FindTypeInGame (string fullName) {
      return GameAssembly.GetSafeTypes().FirstOrDefault(t => {
         return t.FullName.Equals(fullName);
      });
   }

   public static Type FindType (string fullName) {
      return AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetSafeTypes()).FirstOrDefault(t => {
         return t.FullName.Equals(fullName);
      });
   }

   private static IEnumerable<Type> GetSafeTypes (this Assembly assembly) {
      try {
         return assembly.GetTypes();
      }
      catch (ReflectionTypeLoadException e) {
         return e.Types.Where(x => x != null);
      }
      catch (Exception e) {
         return null;
      }
   }

   private static Assembly GameAssembly {
      get {
         if (_gameAssembly == null) {
            _gameAssembly = FindType("KTInputManager").Assembly;
         }

         return _gameAssembly;
      }
   }

   private static Assembly _gameAssembly = null;
}