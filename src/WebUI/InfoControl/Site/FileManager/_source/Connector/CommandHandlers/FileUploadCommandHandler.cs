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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using CKFinder.Settings;
using InfoControl.IO.Compression;

namespace CKFinder.Connector.CommandHandlers
{
    internal class FileUploadCommandHandler : CommandHandlerBase
    {
        private const string themeExtension = ".theme.htm";
        private const string packExtension = ".pack";

        public override void SendResponse(HttpResponse response)
        {
            int iErrorNumber = 0;
            string sFileName = "";

            try
            {
                CheckConnector();
                CheckRequest();

                HttpPostedFile oFile = HttpContext.Current.Request.Files["NewFile"];
                if (oFile == null)
                {
                    ConnectorException.Throw(Errors.UploadedCorrupt);
                    return;
                }

                if (!CurrentFolder.CheckAcl(AccessControlRules.FileUpload))
                {
                    ConnectorException.Throw(Errors.Unauthorized);
                }

                sFileName = Path.GetFileName(oFile.FileName.ToLower());
                if (Connector.CheckFileName(sFileName) && !Config.Current.CheckIsHiddenFile(sFileName))
                {
                    //
                    // Replace dots in the name with underscores (only one dot can be there... security issue).
                    //
                    if (Config.Current.ForceSingleExtension)
                        sFileName = Regex.Replace(sFileName, @"\.(?![^.]*$)", "_", RegexOptions.None);

                    if (!Config.Current.CheckSizeAfterScaling && CurrentFolder.ResourceTypeInfo.MaxSize > 0 && oFile.ContentLength > CurrentFolder.ResourceTypeInfo.MaxSize)
                        ConnectorException.Throw(Errors.UploadedTooBig);

                    //
                    //  Validate Extension
                    //
                    string sExtension = Path.GetExtension(oFile.FileName);
                    sExtension = sExtension.TrimStart('.');

                    if (!CurrentFolder.ResourceTypeInfo.CheckExtension(sExtension))
                        ConnectorException.Throw(Errors.InvalidExtension);

                    //if (Config.Current.CheckIsNonHtmlExtension(sExtension) && !CheckNonHtmlFile(oFile))
                    //    ConnectorException.Throw(Errors.UploadedWrongHtmlFile);

                    //
                    // Map the virtual path to the local server path.
                    //
                    string sServerDir = CurrentFolder.ServerPath;
                    string sFileNameNoExt = Path.GetFileNameWithoutExtension(sFileName);
                    int iCounter = 0;

                    while (true)
                    {
                        string sFilePath = Path.Combine(sServerDir, sFileName);

                        //if (File.Exists(sFilePath))
                        //{
                        //    iCounter++;
                        //    sFileName = string.Format("{0} ({1}){2}",
                        //                              sFileNameNoExt,
                        //                              iCounter,
                        //                              Path.GetExtension(oFile.FileName));

                        //    iErrorNumber = Errors.UploadedFileRenamed;
                        //    continue;
                        //}

                        //
                        // Save file
                        //
                        oFile.SaveAs(sFilePath);
                        try
                        {
                            if (oFile.FileName.ToLower().Contains(themeExtension))
                            {
                                TransformThemeFileInMasterPage(sFilePath);
                            }
                            else if (oFile.FileName.ToLower().Contains(packExtension))
                            {
                                var zipFile = new ZipFile(sFilePath);

                                zipFile.UnZipping += (sender, e) =>
                                                     {
                                                         string extension = Path.GetExtension(e.FullPath.ToLower()).Trim(new[] { '.' });
                                                         if (!String.IsNullOrEmpty(extension) && !CurrentFolder.ResourceTypeInfo.CheckExtension(extension))
                                                             e.Cancel = true;
                                                     };

                                zipFile.UnZipped += (sender, e) =>
                                                    {
                                                        if (e.FullPath.ToLower().Contains(themeExtension))
                                                            TransformThemeFileInMasterPage(e.FullPath);
                                                    };
                                zipFile.Extract(sServerDir);
                            }


                            if (ImageTools.IsImageExtension(sExtension))
                            {
                                if (Config.Current.SecureImageUploads && !ImageTools.ValidateImage(sFilePath))
                                {
                                    File.Delete(sFilePath);
                                    ConnectorException.Throw(Errors.UploadedCorrupt);
                                }

                                Images imagesSettings = Config.Current.Images;

                                if (imagesSettings.MaxHeight <= 0 || imagesSettings.MaxWidth <= 0)
                                    break;

                                ImageTools.ResizeImage(sFilePath, sFilePath, imagesSettings.MaxWidth, imagesSettings.MaxHeight, true, imagesSettings.Quality);
                            }

                            if (Config.Current.CheckSizeAfterScaling && CurrentFolder.ResourceTypeInfo.MaxSize > 0)
                            {
                                long fileSize = new FileInfo(sFilePath).Length;
                                if (fileSize > CurrentFolder.ResourceTypeInfo.MaxSize)
                                {
                                    File.Delete(sFilePath);
                                    ConnectorException.Throw(Errors.UploadedTooBig);
                                }
                            }

                        }
                        catch (ArgumentException ex)
                        {
                            iErrorNumber = Errors.UploadedWrongThemeFile;
                        }

                        break;
                    }
                }
                else
                    ConnectorException.Throw(Errors.InvalidName);
            }
            catch (ConnectorException connectorException)
            {
                iErrorNumber = connectorException.Number;
            }
            catch (Exception ex)
            {
                ex.GetHashCode();
                iErrorNumber = Errors.Unknown;
            }

            response.Clear();

            response.Write("<script type=\"text/javascript\">");
            response.Write(GetJavaScriptCode(iErrorNumber, sFileName, CurrentFolder.Url + sFileName));
            response.Write("</script>");

            response.End();
        }

        protected virtual string GetJavaScriptCode(int errorNumber, string fileName, string fileUrl)
        {
            switch (errorNumber)
            {
                case Errors.None:
                case Errors.UploadedFileRenamed:
                    return "window.parent.OnUploadCompleted(" + errorNumber + ",'" + fileName.Replace("'", "\\'") + "') ;";
                default:
                    return "window.parent.OnUploadCompleted(" + errorNumber + ") ;";
            }
        }


        /// <summary>
        /// Transform .theme file in .master MasterPage
        /// </summary>
        private void TransformThemeFileInMasterPage(string sFilePath)
        {
            //
            // Remove runat=server for security reasons
            //
            string runatServer = "runat=\"server\"";
            string fileContent = File.ReadAllText(sFilePath, Encoding.Default);
            var stringBuilder = new StringBuilder(fileContent);
            stringBuilder.Replace(runatServer, "");

            //
            // Set header master page
            // 
            stringBuilder.Insert(0,
                                 @"<%@ Master Language='C#' AutoEventWireup='true' %>
                <%@ Register Src='~/site/_modules/Menu.ascx' TagName='Menu' TagPrefix='vfx' %>
                <%@ Register Src='~/site/_modules/Categories.ascx' TagName='Categories' TagPrefix='vfx' %>
                <%@ Register Src='~/site/_modules/FeedReader.ascx' TagName='FeedReader' TagPrefix='vfx' %>
                <%@ Register Src='~/site/_modules/Products.ascx' TagName='Products' TagPrefix='vfx' %>
                <%@ Register Src='~/site/_modules/Search.ascx' TagName='Search' TagPrefix='vfx' %>
                <%@ Register Src='~/site/_modules/Tags.ascx' TagName='Tags' TagPrefix='vfx' %>
                <%@ Register Src='~/site/_modules/PageCategories.ascx' TagName='PageCategories' TagPrefix='vfx' %>
                <%@ Register Src='~/App_Modules/AccessControl/Login.ascx' TagName='Login'  TagPrefix='vfx' %>
                <%@ Register Src='~/site/_modules/WebPage.ascx' TagName='WebPage'  TagPrefix='vfx' %>");

            //
            // Set head as runat=server
            //
            if (!fileContent.Contains("<head>"))
                throw new ArgumentException("The Head tag is missing in HTML!");

            stringBuilder.Replace("<head", "<head " + runatServer);
            stringBuilder.Replace("</head>", @"
<meta name='generator' content='Vivina InfoControl' />
<asp:ContentPlaceHolder ID='head' runat='server'></asp:ContentPlaceHolder></head>");

            //
            // Adds Form And ScriptManager in page
            //
            stringBuilder.Replace("<body>",
                                 @"<body>
                                   <form id='form1' runat='server'>
                                    <asp:ScriptManager runat='server' EnablePageMethods='true'>
                                        <scripts>
                                            <asp:ScriptReference Path='~/App_Shared/js/scriptcombiner.ascx' />
                                        </scripts>
                                    </asp:ScriptManager>");

            //
            // Conteudo
            //
            if (!fileContent.Contains("<conteudo/>") && !fileContent.Contains("<conteudo />"))
                throw new ArgumentException("The Content tag '<conteudo>' is missing in HTML!");

            stringBuilder.Replace("<conteudo", "<asp:ContentPlaceHolder ID='ContentPlaceHolder' runat='server' ");

            //
            // Categorias
            //
            stringBuilder.Replace("<categorias", "<vfx:Categories  runat='server' ");

            //
            // Produtos
            //
            stringBuilder.Replace("<produtos", "<vfx:Products  runat='server' ");

            //
            // FeedReader
            //
            stringBuilder.Replace("<rss", "<vfx:FeedReader  runat='server' ");

            //
            // WebPage
            //
            stringBuilder.Replace("<paginas", "<vfx:WebPage  runat='server' ");

            //
            // Login
            //
            stringBuilder.Replace("<login", "<vfx:Login  runat='server' ");

            //
            // Tags
            //
            stringBuilder.Replace("<tags", "<vfx:Tags  runat='server' ");

            //
            // Page Categories
            //
            stringBuilder.Replace("<categoriasDePagina", "<vfx:PageCategories  runat='server' ");

            //
            // Search
            //
            stringBuilder.Replace("<busca", "<vfx:Search  runat='server'  ");

            //
            // Ends tag Form
            //
            stringBuilder.Replace("</body>", "</form></body>");

            //
            // Fix img src
            //
            stringBuilder.Replace("img src=\"", "img src=\"/site/" + Config.Current.ResourceTypes[0].Name + "/");

            File.Delete(sFilePath);
            File.WriteAllText(sFilePath.Replace(themeExtension, ".master"), stringBuilder.ToString(), Encoding.UTF8);
        }
    }
}