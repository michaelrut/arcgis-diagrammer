/*=============================================================================
 * 
 * Copyright � 2007 ESRI. All rights reserved. 
 * 
 * Use subject to ESRI license agreement.
 * 
 * Unpublished�all rights reserved.
 * Use of this ESRI commercial Software, Data, and Documentation is limited to
 * the ESRI License Agreement. In no event shall the Government acquire greater
 * than Restricted/Limited Rights. At a minimum Government rights to use,
 * duplicate, or disclose is subject to restrictions as set for in FAR 12.211,
 * FAR 12.212, and FAR 52.227-19 (June 1987), FAR 52.227-14 (ALT I, II, and III)
 * (June 1987), DFARS 227.7202, DFARS 252.227-7015 (NOV 1995).
 * Contractor/Manufacturer is ESRI, 380 New York Street, Redlands,
 * CA 92373-8100, USA.
 * 
 * SAMPLE CODE IS PROVIDED "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 * PARTICULAR PURPOSE, ARE DISCLAIMED.  IN NO EVENT SHALL ESRI OR CONTRIBUTORS
 * BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) SUSTAINED BY YOU OR A THIRD PARTY, HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT; STRICT LIABILITY; OR TORT ARISING
 * IN ANY WAY OUT OF THE USE OF THIS SAMPLE CODE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE TO THE FULL EXTENT ALLOWED BY APPLICABLE LAW.
 * 
 * =============================================================================*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Printing;
using System.Globalization;
using ESRI.ArcGIS.Diagrammer.Properties;

namespace ESRI.ArcGIS.Diagrammer {
    public class PrinterPaperSizeConverter : TypeConverter {
        public PrinterPaperSizeConverter() { }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) {
            return true;
        }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) {
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }

            // Get Settings
            DiagramPrinterSettings settingsPrinter = context.Instance as DiagramPrinterSettings;
            if (settingsPrinter == null) { return null; }

            // Get Printer Name
            string printerName = settingsPrinter.PrinterName;

            PrintDocument printerDocument = new PrintDocument();
            PrinterSettings printerSettings = printerDocument.PrinterSettings;
            printerSettings.PrinterName = string.IsNullOrEmpty(printerName) ? null : printerName;

            //
            List<string> list = new List<string>();
            foreach (PaperSize paperSize in printerSettings.PaperSizes) {
                list.Add(paperSize.PaperName);
            }

            // Sort
            list.Sort();
            list.Insert(0, Resources.TEXT_DEFAULT_BR);

            // Return List
            StandardValuesCollection svc = new StandardValuesCollection(list);
            return svc;
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
            if (context == null) { return null; }
            if (context.Instance == null) { return null; }

            if (value.GetType() == typeof(string)) {
                string text = (string)value;

                // If NULL return empty string
                if (string.IsNullOrEmpty(text)) {
                    return string.Empty;
                }

                // If "(default)" return empty string
                if (text == Resources.TEXT_DEFAULT_BR) {
                    return string.Empty;
                }

                return text;
            }

            return base.ConvertFrom(context, culture, value);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor)) {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (destinationType == null) {
                throw new ArgumentNullException("destinationType");
            }
            if (destinationType == typeof(string)) {
                if (value.GetType() == typeof(string)) {
                    string text = (string)value;
                    if (string.IsNullOrEmpty(text)) {
                        return Resources.TEXT_DEFAULT_BR;
                    }
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) {
            return true;
        }
    }
}
