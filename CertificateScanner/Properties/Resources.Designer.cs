﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.18047
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CertificateScanner.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CertificateScanner.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
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
        ///   Ищет локализованную строку, похожую на [Constant]
        ///debugDataLevel=true
        ///EncryptLog=false
        ///baseUser=user
        ///[Scan]
        ///barcodetype=ean13
        ///dpi=300
        ///color=Color
        ///#&quot;BlackWhite&quot;, &quot;Color&quot;, &quot;Gray&quot;. Be carefully, human photo has color
        ///deviceuuid=
        ///auto=true
        ///barNumber=true
        ///[MainRegion]
        ///x=168
        ///y=472
        ///width=803
        ///height=1296
        ///tolerance=0
        ///[Regionsign]
        ///x=265
        ///y=1491
        ///width=588
        ///height=176
        ///tolerance=0
        ///[Regionphoto]
        ///x=222
        ///y=529
        ///width=453
        ///height=605
        ///tolerance=0
        ///[Regionbar]
        ///x=217
        ///y=1144
        ///width=363
        ///height=296
        ///tolerance=0
        ///[Save]
        ///path=D:\photo_scan
        ///# [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string config {
            get {
                return ResourceManager.GetString("config", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на [Messages]
        ///qrFail=Не вдалось сканувати QR-код
        ///qrProgress=Йде сканування QR коду...
        ///saveProgress=Йде збереження зображень...
        ///jp2CdntLoadSource=Не можу завантажити джерело для кодування в JP2
        ///resultSaved=Інформацію збережено
        ///resultCntSave=Інформацію НЕ збережено. Перевірте номер заявки та INI файл на символи, заборонені файловою системою.
        ///resultCntSaveNullDir=Інформацію НЕ збережено. Теки призначення({0}) не існує.
        ///sertificateNumberIsEmpty=Спершу введіть номер заявки
        ///scannerUUIDDroped=Тепер можна виб [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string messages {
            get {
                return ResourceManager.GetString("messages", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;nlog xmlns=&quot;http://www.nlog-project.org/schemas/NLog.xsd&quot;
        ///      xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot;&gt;
        ///
        ///  &lt;!-- make sure to set &apos;Copy To Output Directory&apos; option for this file --&gt;
        ///  &lt;!-- go to http://nlog-project.org/wiki/Configuration_file for more information --&gt;
        ///
        ///  &lt;!-- Путь к log директории --&gt;
        ///  &lt;variable name=&quot;logDir&quot; value=&quot;${basedir}/log/${date:format=yyyy-MM-dd}&quot;/&gt;
        ///
        ///  &lt;!-- Настройка Layout --&gt;
        ///  &lt;variable name=&quot;shortLayout&quot; value=&quot; [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string NLog {
            get {
                return ResourceManager.GetString("NLog", resourceCulture);
            }
        }
    }
}
