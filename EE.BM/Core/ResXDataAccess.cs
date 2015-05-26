using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Globalization;

namespace EE.BM
{
    public class ResXDataAccess
    {
        private string folderLocation;

        private Dictionary<string, string> dictResX = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the ResXDataAccess class.
        /// </summary>
        /// <param name="folderLocation">The location where to load the resource file.</param>
        public ResXDataAccess(string folderLocation)
        {
            this.folderLocation = folderLocation;
        }

        /// <summary>
        /// Loads a res x file.
        /// </summary>
        /// <param name="locale">The locale to load.</param>
        /// <returns>The xml containing the resx key value pairs.</returns>
        public void Load(string locale)
        {
            string fileName = string.Format(CultureInfo.InvariantCulture, "{0}.resx", Path.Combine(this.folderLocation, locale));

            if (File.Exists(fileName))
            {
                XmlDocument resXFileName = new XmlDocument();
                resXFileName.Load(fileName);

                Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (XmlNode node in resXFileName.SelectNodes("//data"))
                {
                    XPathNavigator nav = node.CreateNavigator();

                    string key = nav.GetAttribute("name","");
                    string value = string.Empty;
                    XPathNodeIterator iter = nav.SelectChildren(XPathNodeType.Element);
                    while (iter.MoveNext())
                    {
                        value = iter.Current.Value;
                        break;
                    }
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        dictResX.Add(key, value);
                    }
                }
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            value = string.Empty;

            if (dictResX == null || dictResX.Count == 0) return false;

            if (dictResX.TryGetValue(key, out value))
            {
                return true;
            }
            
            return false;
        }
    }
}
