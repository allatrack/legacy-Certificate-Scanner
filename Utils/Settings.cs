﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CertificateScanner.Utils
{
    /// <summary>
    /// Class with messages and constants
    /// </summary>
    public sealed class Settings
    {
        #region constructor

        private Settings()
        {
            //Load Constants
            LoadSectionFromIni(AppDomain.CurrentDomain.BaseDirectory + "config.ini", "Constant", ref constant);

            //Load Messages
            LoadSectionFromIni(AppDomain.CurrentDomain.BaseDirectory + "messages.ini", "Messages", ref messages);
        }

        #endregion

        #region Implementation Singleton

        private static readonly Settings _instance = new Settings();

        public static Settings Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Fill Dictionary<string, string> variable from section in ini file. Default = "" 
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="section">Section of variable</param>
        /// <param name="variable">Variable to save</param>
        private static void LoadSectionFromIni(string path, string section, ref Dictionary<string, string> variable)
        {
            //Check for exist file Config.ini
            if (System.IO.File.Exists(path))
            {
                //Connect to Ini File "settings.ini" in current directory
                Ini.IniInterface oIni = new Ini.IniInterface(path);
                Array itemNames = Array.CreateInstance(typeof(string), 0);
                oIni.ReadValues(section, ref itemNames);
                foreach (string itemName in itemNames)
                {
                    try
                    {
                        variable.Add(itemName, oIni.ReadValue(section, itemName));
                    }
                    catch  // I can`t do anything because this event fire before core
                    {
                        
                    }
                }
            }
            else
            {
                //proposition to restore section.ini
                //TODO Сделать восстановление ini файлов из бекапа
            }

        }

        #endregion

        #region private variable

        private Dictionary<string, string> constant = new Dictionary<string, string>();
        private Dictionary<string, string> messages = new Dictionary<string, string>();

        #endregion

        #region Geters

        /// <summary>
        /// Application constant. Getting from settings.ini
        /// </summary>
        public string Constant(string Key)
        {
            string value = Key;
            return (constant.TryGetValue(Key, out value)) ? value : Key;
        }

        /// <summary>
        /// Application messages and errors. Getting from settings.ini
        /// </summary>
        public string Messages(string Key)
        {
            string value = Key;
            return (messages.TryGetValue(Key, out value)) ? value : Key;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Application messages and errors. Getting from settings.ini
        /// </summary>
        public Dictionary<string, string> MessagesArray
        {
            get
            {
                return messages;
            }
        }

        /// <summary>
        /// Application constant and errors. Getting from settings.ini
        /// </summary>
        public Dictionary<string, string> ConstantArray
        {
            get
            {
                return constant;
            }
        }

        #endregion
    }
}