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
    class ResXDataAccess
    {
        private string folderLocation;

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
        public IXPathNavigable Load(string locale)
        {
            string fileName = string.Format(CultureInfo.InvariantCulture, "{0}.resx", Path.Combine(this.folderLocation, locale));

            if (File.Exists(fileName))
            {
                XmlDocument resXFileName = new XmlDocument();
                resXFileName.Load(fileName);

                return resXFileName;
            }

            return null;
        }
    }
}
