﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace adhdb.language.en {
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
    internal class CommandManipulator {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CommandManipulator() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("adhdb.language.en.CommandManipulator", typeof(CommandManipulator).Assembly);
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
        ///   Looks up a localized string similar to Unknown error!.
        /// </summary>
        internal static string AddNewCommandError {
            get {
                return ResourceManager.GetString("AddNewCommandError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please input all relevant arguments. For example: !addcommand ping Pong Pong Pong..
        /// </summary>
        internal static string AddNewCommandMissingArguments {
            get {
                return ResourceManager.GetString("AddNewCommandMissingArguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only server admins can use this command..
        /// </summary>
        internal static string AddNewCommandMissingRights {
            get {
                return ResourceManager.GetString("AddNewCommandMissingRights", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown error!.
        /// </summary>
        internal static string ChangeLanguageError {
            get {
                return ResourceManager.GetString("ChangeLanguageError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only server admins can use this command..
        /// </summary>
        internal static string ChangeLanguageMissingRights {
            get {
                return ResourceManager.GetString("ChangeLanguageMissingRights", resourceCulture);
            }
        }
    }
}
