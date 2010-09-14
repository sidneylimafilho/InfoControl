/*
 * CKFinder
 * ========
 * http://www.ckfinder.com
 * Copyright (C) 2007-2008 Frederico Caldeira Knabben (FredCK.com)
 *
 * The software, this file and its contents are subject to the CKFinder
 * License. Please read the license.txt file before using, installing, copying,
 * modifying or distribute this file or part of its contents. The contents of
 * this file is part of the Source Code of CKFinder.
 */

using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace CKFinder.Connector.CommandHandlers
{
    internal class GetFilesCommandHandler : XmlCommandHandlerBase
    {
        public GetFilesCommandHandler() : base() {}

        protected override void BuildXml()
        {
            if (!CurrentFolder.CheckAcl(AccessControlRules.FileView))
            {
                ConnectorException.Throw(Errors.Unauthorized);
            }

            // Map the virtual path to the local server path.
            string sServerDir = CurrentFolder.ServerPath;
            bool bShowThumbs = Request.QueryString["showThumbs"] != null && Request.QueryString["showThumbs"].ToString().Equals("1");

            // Create the "Files" node.
            XmlNode oFilesNode = XmlUtil.AppendElement(ConnectorNode, "Files");

            var oDir = new DirectoryInfo(sServerDir);
            FileInfo[] aFiles = oDir.GetFiles();

            for (int i = 0; i < aFiles.Length; i++)
            {
                FileInfo oFile = aFiles[i];
                string sExtension = Path.GetExtension(oFile.Name);

                if (Config.Current.CheckIsHiddenFile(oFile.Name))
                    continue;

                //if ( !this.CurrentFolder.ResourceTypeInfo.CheckExtension( sExtension ) )
                //    continue;

                Decimal iFileSize = Math.Round((Decimal) oFile.Length/1024);
                if (iFileSize < 1 && oFile.Length != 0) iFileSize = 1;

                // Create the "File" node.
                XmlNode oFileNode = XmlUtil.AppendElement(oFilesNode, "File");
                XmlUtil.SetAttribute(oFileNode, "name", oFile.Name);
                XmlUtil.SetAttribute(oFileNode, "date", oFile.LastWriteTime.ToString("yyyyMMddHHmm"));
                if (Config.Current.Thumbnails.Enabled
                    && (Config.Current.Thumbnails.DirectAccess || bShowThumbs)
                    && ImageTools.IsImageExtension(sExtension.TrimStart('.')))
                {
                    bool bFileExists = File.Exists(Path.Combine(CurrentFolder.ThumbsServerPath, oFile.Name));
                    if (bFileExists)
                        XmlUtil.SetAttribute(oFileNode, "thumb", oFile.Name);
                    else if (bShowThumbs)
                        XmlUtil.SetAttribute(oFileNode, "thumb", "?" + oFile.Name);
                }
                XmlUtil.SetAttribute(oFileNode, "size", iFileSize.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}