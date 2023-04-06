﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Watchdog.Bot.Strings {
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
    internal class Phrases {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Phrases() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Watchdog.Bot.Strings.Phrases", typeof(Phrases).Assembly);
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
        ///   Looks up a localized string similar to Ban.
        /// </summary>
        internal static string Ban {
            get {
                return ResourceManager.GetString("Ban", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ID.
        /// </summary>
        internal static string ID {
            get {
                return ResourceManager.GetString("ID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Kick.
        /// </summary>
        internal static string Kick {
            get {
                return ResourceManager.GetString("Kick", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Moderator.
        /// </summary>
        internal static string Moderator {
            get {
                return ResourceManager.GetString("Moderator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to :warning: You&apos;ve been warned by moderator **{0}** on the server **{1}**! Reason: **{2}**. Current warnings count: **{3}**. ID: **{4}**..
        /// </summary>
        internal static string Notification_Warning {
            get {
                return ResourceManager.GetString("Notification_Warning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to :envelope: Moderator **{0}** has removed your warning with ID **{1}** on the server **{2}**. Current warnings count: **{3}**..
        /// </summary>
        internal static string Notification_WarningDeletion {
            get {
                return ResourceManager.GetString("Notification_WarningDeletion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reason.
        /// </summary>
        internal static string Reason {
            get {
                return ResourceManager.GetString("Reason", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Timestamp.
        /// </summary>
        internal static string Timestamp {
            get {
                return ResourceManager.GetString("Timestamp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Until.
        /// </summary>
        internal static string Until {
            get {
                return ResourceManager.GetString("Until", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User.
        /// </summary>
        internal static string User {
            get {
                return ResourceManager.GetString("User", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        internal static string Warning {
            get {
                return ResourceManager.GetString("Warning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number of warnings.
        /// </summary>
        internal static string WarningCount {
            get {
                return ResourceManager.GetString("WarningCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Succesfully removed warning **{0}** from user **{1}**. Current warning count: **{2}**..
        /// </summary>
        internal static string WarningDeletionModeratorConfirmation {
            get {
                return ResourceManager.GetString("WarningDeletionModeratorConfirmation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully warned member **{0}**. Current warnings count: **{1}**..
        /// </summary>
        internal static string WarningModeratorConfirmation {
            get {
                return ResourceManager.GetString("WarningModeratorConfirmation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning with ID **{0}** does not exist..
        /// </summary>
        internal static string WarningNotFound {
            get {
                return ResourceManager.GetString("WarningNotFound", resourceCulture);
            }
        }
    }
}
