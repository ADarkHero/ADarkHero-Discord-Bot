﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace adhdb.language.de {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SQLHelper {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SQLHelper() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("adhdb.language.de.SQLHelper", typeof(SQLHelper).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fehler beim Anlegen des commands..
        /// </summary>
        internal static string InsertNewCommandError {
            get {
                return ResourceManager.GetString("InsertNewCommandError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Das folgende command wurde erfolgreich angelegt:.
        /// </summary>
        internal static string InsertNewCommandSuccess {
            get {
                return ResourceManager.GetString("InsertNewCommandSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unbekannter Fehler..
        /// </summary>
        internal static string ListAllFunctionsError {
            get {
                return ResourceManager.GetString("ListAllFunctionsError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Die angegebene Sprache existiert nicht..
        /// </summary>
        internal static string SetLanguageLanguageNotExistent {
            get {
                return ResourceManager.GetString("SetLanguageLanguageNotExistent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Die Sprache wurde erfolgreich angepasst!.
        /// </summary>
        internal static string SetLanguageLanguageSet {
            get {
                return ResourceManager.GetString("SetLanguageLanguageSet", resourceCulture);
            }
        }
    }
}
